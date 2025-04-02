using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client.Enums;
using QuickBaseApi.Client.Factories;
using QuickBaseApi.Client.Models;
using static QuickBaseApi.Client.Utils.FieldHelper;
using static QuickBaseApi.Client.Factories.FieldFactory;
using static QuickBaseApi.Client.Factories.CreateRecordFactory;

namespace QuickBaseApi.Tests
{
    public class DeleteRecordTests : BaseTest
    {
        [Test]
        public async Task DeleteRecord_ByFieldId_ShouldSucceed()
        {            
            var recordToDelete = GenerateRecord(TasksFields, f => f.Required == true);
            var field = GetRandomField(TasksFields, f => f.Required == false);
            var value = GenerateRandomValueForField(field);
            recordToDelete[field.Id.ToString()] = new FieldValueModel { Value = value };

            var createRequest = CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequest);
            var createdRecordId = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content).Metadata.CreatedRecordIds.First();

            var deleteRequest = DeleteRecordFactory.DeleteRecord(TasksTable.Id, fieldId: "3", QueryOperator.EX, createdRecordId);

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteRequest);
            var deleteResult = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResult.NumberDeleted, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task DeleteRecord_ByFieldValue_ShouldSucceed()
        {
            var recordToDelete = GenerateRecord(TasksFields, f => f.Required == true);
            var field = GetRandomField(TasksFields, f => f.Required == false);
            var value = GenerateRandomValueForField(field);
            recordToDelete[field.Id.ToString()] = new FieldValueModel { Value = value };

            var createRequest = CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequest);
            var createdRecordId = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content).Metadata.CreatedRecordIds.First();

            var deleteRequest = DeleteRecordFactory.DeleteRecord(TasksTable.Id, fieldId: field.Id, QueryOperator.EX, value);
            var deleteRequestSerialized = JsonConvert.SerializeObject(deleteRequest, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteRequest);
            var deleteResult = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResult.NumberDeleted, Is.EqualTo(1));
            });
        }
    }
}
