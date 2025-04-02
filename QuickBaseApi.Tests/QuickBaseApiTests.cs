using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client;
using QuickBaseApi.Client.Factories;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Utils;
using RestSharp;
using System.Net;
using static QuickBaseApi.Client.Factories.QuickBaseFieldFactory;

namespace QuickBaseApi.Tests
{
    [TestFixture]
    public class QuickBaseApiTests
    {
        private QuickBaseClient? _quickBaseClient;
        private RestClient? _restClient;
        private QuickBaseConfig? _config;
        private List<QuickBaseTable> _tables;
        private QuickBaseTable? _tableProjects;
        private List<QuickBaseFieldModel> _fieldsProjects;
        private QuickBaseTable? _tableTasks;
        private List<QuickBaseFieldModel> _fieldsTasks;

        [OneTimeSetUp]
        public async Task OneTimeSetUpAsync()
        {
            _config = Configuration.Config.GetSection("QuickBase").Get<QuickBaseConfig>();
            _quickBaseClient = new QuickBaseClient(_config);

            _tables = await _quickBaseClient.GetTablesAsync(_config.AppId);
            _tableProjects = _tables.FirstOrDefault(t => t.Name.Equals("Projects", StringComparison.OrdinalIgnoreCase));
            _tableTasks = _tables.FirstOrDefault(t => t.Name.Equals("Tasks", StringComparison.OrdinalIgnoreCase));

            _fieldsProjects = await _quickBaseClient.GetFieldsByTableName(_tables.Where(t => t.Name == "Projects").First().Id);
            _fieldsTasks = await _quickBaseClient.GetFieldsByTableName(_tables.Where(t => t.Name == "Tasks").First().Id);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task AddMutipleProjectRecords_AllFields_ShouldSucceed()
        {
            var records = GenerateValidRecordsForTable(_fieldsProjects);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(records.First());
            var requestBody = RecordFactory.BuildMultipleRecords(_tableProjects.Id, records, fieldsToReturn.ToArray());
            var requestBodySerialized = JsonConvert.SerializeObject(requestBody, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var response = await _quickBaseClient.AddRecordAsync(requestBody);
            var responseSerialized = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(records.Count()));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(records.Count()));
            Assertions.AssertFieldValues(response.Content.Data, records, fieldsToReturn);
        }

        [Test]
        public async Task AddMutipleTaskRecords_AllFields_ShouldSucceed()
        {
            var records = GenerateValidRecordsForTable(_fieldsTasks);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(records.First());
            var requestBody = RecordFactory.BuildMultipleRecords(_tableTasks.Id, records, fieldsToReturn.ToArray());

            var response = await _quickBaseClient.AddRecordAsync(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(records.Count()));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(records.Count()));
            Assertions.AssertFieldValues(response.Content.Data, records, fieldsToReturn);
        }
        
        [Test]
        public async Task AddRecord_AllFields_ShouldSucceed()
        {
            var record = GenerateValidRecordForTable(_fieldsTasks, RequiredFieldFilter.All);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record, fieldsToReturn.ToArray());

            var response = await _quickBaseClient.AddRecordAsync(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(1));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(1));
            Assertions.AssertFieldValues(response.Content.Data, new List<Dictionary<string, FieldValueModel>> { record }, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_RequredFields_ShouldSucceed()
        {
            var record = GenerateValidRecordForTable(_fieldsTasks, RequiredFieldFilter.OnlyRequired);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record, fieldsToReturn.ToArray());

            var response = await _quickBaseClient.AddRecordAsync(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(1));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(1));
            Assertions.AssertFieldValues(response.Content.Data, new List<Dictionary<string, FieldValueModel>> { record }, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_OptionalFields_ShoudReturnMultiStatus()
        {
            var record = GenerateValidRecordForTable(_fieldsTasks, RequiredFieldFilter.OnlyOptional);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record, fieldsToReturn.ToArray());

            var response = await _quickBaseClient.AddRecordAsync(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MultiStatus));
            Assert.That(response.Content.Metadata.LineErrors.Count(), Is.EqualTo(1));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(0));
            Assertions.AssertRequiredFieldsMissingErrorMessage(response.Content, _fieldsTasks);
        }

        [Test] 
        public async Task AddRecord_WithIncompatibleFieldValues_ShouldReturnMultiStatus() 
        {
            var (requiredFields, invalidFields) = GenerateInvalidRecordForTable(_fieldsTasks);
            var record = requiredFields.Concat(invalidFields).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record, fieldsToReturn.ToArray());
            var jsonBodySerialized = JsonConvert.SerializeObject(requestBody, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var response = await _quickBaseClient.AddRecordAsync(requestBody);
            var responseSerialized = JsonConvert.SerializeObject(response.Content, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MultiStatus));
            Assert.That(response.Content.Metadata.LineErrors.Count(), Is.EqualTo(1));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(1));
            Assertions.AssertIncompatibleFieldsErrorMessage(response.Content, invalidFields);
        }

        [Test]
        public async Task AddRecord_WithInvalidUserToken_ShouldReturnUnauthorized()
        {
            var token = "Wrong User Token";
            var record = GenerateValidRecordForTable(_fieldsTasks);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record);

            var response = await _quickBaseClient.AddRecordAsync<ErrorResponseModel>(requestBody, token);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(response.Content.Message, Is.EqualTo(Constants.InvalidUserToken.Message));
            Assert.That(response.Content.Description, Is.EqualTo(Constants.InvalidUserToken.Description));
        }

        [Test]
        public async Task AddRecord_WithNonExistingTableId_ShouldReturnUnauthorized()
        {
            var invalidTableId = HelperMethods.RandomString(10);
            var record = GenerateValidRecordForTable(_fieldsTasks);
            var requestBody = RecordFactory.BuildSingleRecord(invalidTableId, record);

            var response = await _quickBaseClient.AddRecordAsync<ErrorResponseModel>(requestBody);
            var responseSerialized = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(response.Content.Message, Is.EqualTo(Constants.InvalidUserToken.Message));
            Assert.That(response.Content.Description, Is.EqualTo(Constants.InvalidUserToken.Description));
        }

        [Test]
        public async Task AddRecord_WithInvalidTableId_ShouldReturnErrorMessage()
        {
            var record = GenerateValidRecordForTable(_fieldsTasks, RequiredFieldFilter.OnlyRequired);
            var requestBody = RecordFactory.BuildSingleRecord(null, record);

            var response = await _quickBaseClient.AddRecordAsync<ErrorResponseModel>(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content.Message, Is.EqualTo(Constants.InvalidTableId.Message));
            Assert.That(response.Content.Description, Is.EqualTo(Constants.InvalidTableId.Description));
        }

        [Test]
        public async Task AddRecord_WithEmptyDataProperty_ShouldReturnErrorMessage()
        {
            var requestBody = new RecordModel { To = _tableTasks.Id, Data = new List<Dictionary<string, FieldValueModel>>(), FieldsToReturn = new List<long>().ToArray() };

            var response = await _quickBaseClient.AddRecordAsync<ErrorResponseModel>(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content.Message, Is.EqualTo(Constants.EmptyData.Message));
            Assert.That(response.Content.Description, Is.EqualTo(Constants.EmptyData.Description));
        }

        [Test]
        public async Task AddRecord_WithInvalidDataProperty_ShouldReturnErrorMessage()
        {
            var requestBody = new RecordModel { To = _tableTasks.Id, Data = null, FieldsToReturn = Array.Empty<long>() };

            var response = await _quickBaseClient.AddRecordAsync<ErrorResponseModel>(requestBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content.Message, Is.EqualTo(Constants.InvalidData.Message));
            Assert.That(response.Content.Description, Is.EqualTo(Constants.InvalidData.Description));
        }

        [Test]
        public async Task AddRecord_WithoutHostnameHeader_ShouldReturnErrorMessage()
        {
            _restClient = new RestClient(new RestClientOptions(_config.BaseUrl));
            _restClient.AddDefaultHeader("Authorization", $"QB-USER-TOKEN {_config.UserToken}");

            var record = GenerateValidRecordForTable(_fieldsTasks, RequiredFieldFilter.All);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record);

            var request = new RestRequest("/records", Method.Post).AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task AddRecord_WithoutAuthorizationHeader_ShouldReturnForbidden()
        {
            _restClient = new RestClient(new RestClientOptions(_config.BaseUrl));
            _restClient.AddDefaultHeader("QB-Realm-Hostname", _config.Realm);

            var record = GenerateValidRecordForTable(_fieldsTasks, RequiredFieldFilter.All);
            var requestBody = RecordFactory.BuildSingleRecord(_tableTasks.Id, record);

            var request = new RestRequest("/records", Method.Post).AddJsonBody(requestBody);

            var response = await _restClient.ExecuteAsync(request);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(responseContent.Message, Is.EqualTo(Constants.MissingAuthorizationHeader.Message));
            Assert.That(responseContent.Description, Is.EqualTo(Constants.MissingAuthorizationHeader.Description));
        }
    }
}