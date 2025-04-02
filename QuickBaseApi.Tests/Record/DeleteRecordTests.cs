using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client.Factories;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client;
using static QuickBaseApi.Client.Factories.FieldValueFactory;
using System.Net;

namespace QuickBaseApi.Tests
{
    public class DeleteRecordTests : BaseTest
    {
        [Test]
        public async Task DeleteRecord_ShouldSucceed()
        {
            var recordToDelete = GenerateValidRecordForTable(TasksFields, RequiredFieldFilter.OnlyRequired);
            var createRequestBody = RecordFactory.CreateRecord(TasksTable.Id, recordToDelete);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequestBody);
            var createContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content);
            var recordId = createContent.Metadata.CreatedRecordIds.First();

            var deleteRequest = RecordFactory.DeleteByQuery(TasksTable.Id, recordId.ToString());

            var deleteResponse = await QuickBaseClient.DeleteRecordAsync(deleteRequest);
            var deleteResponseContent = JsonConvert.DeserializeObject<DeleteRecordResponseModel>(deleteResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(deleteResponseContent.NumberDeleted, Is.EqualTo(1));
            });
        }
    }
}
