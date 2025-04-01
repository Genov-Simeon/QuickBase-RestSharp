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
        private QuickBaseClient? _client;
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
            _client = new QuickBaseClient(_config);

            _tables = await _client.GetTablesAsync(_config.AppId);
            _tableProjects = _tables.FirstOrDefault(t => t.Name.Equals("Projects", StringComparison.OrdinalIgnoreCase));
            _tableTasks = _tables.FirstOrDefault(t => t.Name.Equals("Tasks", StringComparison.OrdinalIgnoreCase));

            _fieldsProjects = await _client.GetFieldsByTableName(_tables.Where(t => t.Name == "Projects").First().Id);
            _fieldsTasks = await _client.GetFieldsByTableName(_tables.Where(t => t.Name == "Tasks").First().Id);
        }

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public async Task AddProjectRecord_WithAllFields_ShouldSucceed()
        {
            var recordList = GenerateValidRecordsForTable(_fieldsProjects);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(recordList);
            var jsonBody = RecordFactory.Build(_tableProjects.Id, recordList, fieldsToReturn.ToArray());
            //var jsonBodySerialized = JsonConvert.SerializeObject(jsonBody, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var response = await _client.AddRecordAsync(jsonBody);
            //var responseSerialized = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(recordList.Count()));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(recordList.Count()));
            Assertions.AssertFieldValues(response.Content.Data, recordList, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_WithAllFields_ShouldSucceed()
        {
            var recordList = GenerateValidRecordsForTable(_fieldsTasks);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(recordList);
            var jsonBody = RecordFactory.Build(_tableTasks.Id, recordList, fieldsToReturn.ToArray());

            var response = await _client.AddRecordAsync(jsonBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(recordList.Count()));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(recordList.Count()));
            Assertions.AssertFieldValues(response.Content.Data, recordList, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_WithRequredFields_ShouldSucceed()
        {
            var recordList = GenerateValidRecordsForTable(_fieldsTasks, RequiredFieldFilter.OnlyRequired, 1);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(recordList);
            var jsonBody = RecordFactory.Build(_tableTasks.Id, recordList, fieldsToReturn.ToArray());

            var response = await _client.AddRecordAsync(jsonBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(recordList.Count()));
            Assert.That(response.Content.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(recordList.Count()));
            Assertions.AssertFieldValues(response.Content.Data, recordList, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_OptionalFields_ShoudReturnMultiStatus()
        {
            var recordList = GenerateValidRecordsForTable(_fieldsTasks, RequiredFieldFilter.OnlyOptional, 4);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(recordList);
            var jsonBody = RecordFactory.Build(_tableTasks.Id, recordList, fieldsToReturn.ToArray());

            var response = await _client.AddRecordAsync(jsonBody);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MultiStatus));
            Assert.That(response.Content.Metadata.LineErrors.Count(), Is.EqualTo(recordList.Count()));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(0));
            Assertions.AssertRequiredFieldsErrorMessage(response.Content, _fieldsTasks);
        }

        [Test]
        public async Task AddRecord_WithInvalidUserToken_ShouldReturnUnauthorized()
        {
            var token = "Wrong User Token";
            var recordList = GenerateValidRecordsForTable(_fieldsTasks);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(recordList);
            var jsonBody = RecordFactory.Build(_tableTasks.Id, recordList, fieldsToReturn.ToArray());

            var response = await _client.AddRecordAsync<ErrorResponseModel>(jsonBody, token);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(response.Content.Message, Is.EqualTo(Constants.InvalidUserToken.Message));
            Assert.That(response.Content.Description, Is.EqualTo(Constants.InvalidUserToken.Description));
        }

        [Test] 
        public async Task AddRecord_WithInvalidFieldType_ShouldReturnError() 
        {
            var recordList = GenerateInvalidRecordsForTable(_fieldsTasks, maxEntries: 1);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(recordList);
            var jsonBody = RecordFactory.Build(_tableTasks.Id, recordList, fieldsToReturn.ToArray());
            var jsonBodySerialized = JsonConvert.SerializeObject(jsonBody, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var response = await _client.AddRecordAsync(jsonBody);
            var responseSerialized = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MultiStatus));
            Assert.That(response.Content.Metadata.LineErrors.Count(), Is.EqualTo(recordList.Count()));
            Assert.That(response.Content.Metadata.CreatedRecordIds.Count(), Is.EqualTo(0));
            Assertions.AssertRequiredFieldsErrorMessage(response.Content, _fieldsTasks);
        }

        [Test] public void AddRecord_WithInvalidTableId_ShouldReturnNotFound() { }
        [Test] public void AddRecord_WithEmptyStringValues_ShouldSucceedIfNotRequired() { }
        [Test] public void AddRecord_WithLongTextValues_ShouldTruncateOrError() { }
        [Test] public void AddRecord_WithInvalidFieldId_ShouldReturnError() { }
        [Test] public void AddRecord_WithDuplicateUniqueField_ShouldReturnError() { }
        [Test] public void AddRecord_WithValidData_ShouldReturnRecordId() { }
    }
}