using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client;
using QuickBaseApi.Client.Enums;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Utils;
using RestSharp;
using System.Net;
using static QuickBaseApi.Client.Factories.CreateRecordFactory;
using static QuickBaseApi.Client.Enums.RequiredFieldFilter;

namespace QuickBaseApi.Tests
{
    [TestFixture]
    public class CreateRecordTests : BaseTest
    {
        [Test]
        public async Task AddMutipleProjectRecords_AllFields_ShouldSucceed()
        {
            var records = GenerateRecords(ProjectsFields);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(records.First());
            var requestBody = CreateRecords(ProjectsTable.Id, records, fieldsToReturn.ToArray());

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(responseContent.Metadata.CreatedRecordIds.Count(), Is.EqualTo(records.Count()));
                Assert.That(responseContent.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(records.Count()));
            });

            Assertions.AssertFieldValues(responseContent.Data, records, fieldsToReturn);
        }

        [Test]
        public async Task AddMutipleTaskRecords_AllFields_ShouldSucceed()
        {
            var records = GenerateRecords(TasksFields);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(records.First());
            var requestBody = CreateRecords(TasksTable.Id, records, fieldsToReturn.ToArray());

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(responseContent.Metadata.CreatedRecordIds.Count(), Is.EqualTo(records.Count()));
                Assert.That(responseContent.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(records.Count()));
            });

            Assertions.AssertFieldValues(responseContent.Data, records, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_AllFields_ShouldSucceed()
        {

            var record = GenerateRecord(TasksFields, All);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = CreateRecord(TasksTable.Id, record, fieldsToReturn.ToArray());

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(responseContent.Metadata.CreatedRecordIds.Count(), Is.EqualTo(1));
                Assert.That(responseContent.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(1));
            });

            Assertions.AssertFieldValues(responseContent.Data, new List<Dictionary<string, FieldValueModel>> { record }, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_RequredFields_ShouldSucceed()
        {
            var record = GenerateRecord(TasksFields, OnlyRequired);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = CreateRecord(TasksTable.Id, record, fieldsToReturn.ToArray());

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(responseContent.Metadata.CreatedRecordIds.Count(), Is.EqualTo(1));
                Assert.That(responseContent.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(1));
            });

            Assertions.AssertFieldValues(responseContent.Data, new List<Dictionary<string, FieldValueModel>> { record }, fieldsToReturn);
        }

        [Test]
        public async Task AddRecord_OptionalFields_ShoudReturnMultiStatus()
        {
            var record = GenerateRecord(TasksFields, OnlyOptional);
            var fieldsToReturn = HelperMethods.GetAllFieldIds(record);
            var requestBody = CreateRecord(TasksTable.Id, record, fieldsToReturn.ToArray());

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.MultiStatus));
                Assert.That(responseContent.Metadata.LineErrors.Count(), Is.EqualTo(1));
                Assert.That(responseContent.Metadata.CreatedRecordIds.Count(), Is.EqualTo(0));
            });

            Assertions.AssertRequiredFieldsMissingErrorMessage(responseContent, TasksFields);
        }

        [Test]
        public async Task AddRecord_WithInvalidUserToken_ShouldReturnUnauthorized()
        {
            var token = "Wrong User Token";
            var record = GenerateRecord(TasksFields);
            var requestBody = CreateRecord(TasksTable.Id, record);

            var response = await QuickBaseClient.PostRecordAsync(requestBody, token);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That(responseContent.Message, Is.EqualTo(ErrorResponseMessages.InvalidUserToken.Message));
                Assert.That(responseContent.Description, Is.EqualTo(ErrorResponseMessages.InvalidUserToken.Description));
            });
        }

        [Test]
        public async Task AddRecord_WithNonExistingTableId_ShouldReturnUnauthorized()
        {
            var invalidTableId = HelperMethods.RandomString(10);
            var record = GenerateRecord(TasksFields);
            var requestBody = CreateRecord(invalidTableId, record);

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
                Assert.That(responseContent.Message, Is.EqualTo(ErrorResponseMessages.InvalidUserToken.Message));
                Assert.That(responseContent.Description, Is.EqualTo(ErrorResponseMessages.InvalidUserToken.Description));
            });
        }

        [Test]
        public async Task AddRecord_WithInvalidTableId_ShouldReturnErrorMessage()
        {
            var record = GenerateRecord(TasksFields, OnlyRequired);
            var requestBody = CreateRecord(null, record);

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(responseContent.Message, Is.EqualTo(ErrorResponseMessages.InvalidTableId.Message));
                Assert.That(responseContent.Description, Is.EqualTo(ErrorResponseMessages.InvalidTableId.Description));
            });
        }

        [Test]
        public async Task AddRecord_WithEmptyDataProperty_ShouldReturnErrorMessage()
        {
            var requestBody = new CreateRecordModel { To = TasksTable.Id, Data = new List<Dictionary<string, FieldValueModel>>(), FieldsToReturn = new List<long>().ToArray() };

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(responseContent.Message, Is.EqualTo(ErrorResponseMessages.EmptyData.Message));
                Assert.That(responseContent.Description, Is.EqualTo(ErrorResponseMessages.EmptyData.Description));
            });
        }

        [Test]
        public async Task AddRecord_WithInvalidDataProperty_ShouldReturnErrorMessage()
        {
            var requestBody = new CreateRecordModel { To = TasksTable.Id, Data = null, FieldsToReturn = Array.Empty<long>() };

            var response = await QuickBaseClient.PostRecordAsync(requestBody);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(responseContent.Message, Is.EqualTo(ErrorResponseMessages.InvalidData.Message));
                Assert.That(responseContent.Description, Is.EqualTo(ErrorResponseMessages.InvalidData.Description));
            });
        }

        [Test]
        public async Task AddRecord_WithoutHostnameHeader_ShouldReturnErrorMessage()
        {
            RestClient = new RestClient(new RestClientOptions(Config.BaseUrl));
            RestClient.AddDefaultHeader("Authorization", $"QB-USER-TOKEN {Config.UserToken}");
            var record = GenerateRecord(TasksFields, All);
            var requestBody = CreateRecord(TasksTable.Id, record);

            var request = new RestRequest("/records", Method.Post).AddJsonBody(requestBody);

            var response = await RestClient.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public async Task AddRecord_WithoutAuthorizationHeader_ShouldReturnForbidden()
        {
            RestClient = new RestClient(new RestClientOptions(Config.BaseUrl));
            RestClient.AddDefaultHeader("QB-Realm-Hostname", Config.Realm);
            var record = GenerateRecord(TasksFields, All);
            var requestBody = CreateRecord(TasksTable.Id, record);

            var request = new RestRequest("/records", Method.Post).AddJsonBody(requestBody);

            var response = await RestClient.ExecuteAsync(request);
            var responseContent = JsonConvert.DeserializeObject<ErrorResponseModel>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(responseContent.Message, Is.EqualTo(ErrorResponseMessages.MissingAuthorizationHeader.Message));
                Assert.That(responseContent.Description, Is.EqualTo(ErrorResponseMessages.MissingAuthorizationHeader.Description));
            });
        }
    }
}
