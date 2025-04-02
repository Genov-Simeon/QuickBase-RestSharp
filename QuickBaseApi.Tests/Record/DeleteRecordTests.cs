using System.Net;
using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client.Factories;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client;
using QuickBaseApi.Client.Enums;
using static QuickBaseApi.Client.Factories.CreateRecordFactory;
using static QuickBaseApi.Client.Enums.RequiredFieldFilter;
using QuickBaseApi.Client.Utils;

namespace QuickBaseApi.Tests
{
    public class DeleteRecordTests : BaseTest
    {
        [Test]
        public async Task DeleteRecord_ByFieldId_ShouldSucceed()
        {
            const string fieldValue = "Some text to describe the record";
            var record = GenerateRecord(TasksFields, OnlyRequired);
            var statusFieldId = FieldHelper.GetFieldId(TasksFields, f => f.Label == "Description");
            record[statusFieldId] = new FieldValueModel { Value = fieldValue };

            var createRequest = CreateRecord(TasksTable.Id, record);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequest);
            var createdRecordId = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content)
                .Metadata.CreatedRecordIds.First();

            // Act: Delete using the raw field ID instead of label
            var deleteRequest = DeleteRecordFactory.DeleteByFieldId(TasksTable.Id, createdRecordId);

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteRequest);
            var deleteResult = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResult.NumberDeleted, Is.EqualTo(1));
            });
        }



        [Test]
        public async Task DeleteRecord_ById_ShouldSucceed()
        {
            var recordToDelete = GenerateRecord(TasksFields, All);
            var createRequestBody = CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequestBody);
            var recordId = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content).Metadata.CreatedRecordIds.First();

            var deleteByLabel = DeleteRecordFactory.DeleteByCondition(TasksTable.Id, TasksFields, f => f.Label == "Status", "Completed");

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
            var recordToDelete = GenerateRecord(TasksFields, All);
            var createRequestBody = CreateRecordFactory.CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequestBody);
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
