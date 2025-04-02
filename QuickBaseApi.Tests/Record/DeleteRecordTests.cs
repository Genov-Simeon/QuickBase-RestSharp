using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client.Factories;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Enums;
using static QuickBaseApi.Client.Factories.CreateRecordFactory;
using static QuickBaseApi.Client.Factories.FieldFactory;
using static QuickBaseApi.Client.Utils.FieldHelper;

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

            var deleteRequest = DeleteRecordFactory.DeleteByFieldId(TasksTable.Id, createdRecordId);

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteRequest);
            var deleteResult = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResult.NumberDeleted, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task DeleteRecord_ByCondition_ShouldSucceed()
        {
            var recordToDelete = GenerateRecord(TasksFields, f => f.Required == true);
            var field = GetRandomField(TasksFields, f => f.Required == false);
            var value = GenerateRandomValueForField(field);
            recordToDelete[field.Id.ToString()] = new FieldValueModel { Value = value };

            var createRequest = CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequest);
            var recordId = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content).Metadata.CreatedRecordIds.First();

            var deleteByLabel = DeleteRecordFactory.DeleteByCondition(TasksTable.Id, TasksFields, f => f.Label == field.Label, value);

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteByLabel);
            var deleteResponseContent = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResponseContent.NumberDeleted, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task DeleteRecord_LabelNotHighPriority_ShouldSucceed()
        {
            var recordToDelete = GenerateRecord(TasksFields);
            var createRequest = CreateRecordFactory.CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequest);
            var createContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content);
            var recordId = createContent.Metadata.CreatedRecordIds.First();

            var deleteNotHighPriority = DeleteRecordFactory.DeleteByCondition(TasksTable.Id, TasksFields, f => f.Label == "Priority", "High", QueryOperator.NE);

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteNotHighPriority);
            var deleteResponseContent = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResponseContent.NumberDeleted, Is.EqualTo(1));
            });
        }
    }
}
