using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client.Factories;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Utils;
using QuickBaseApi.Client;
using static QuickBaseApi.Client.Factories.FieldValueFactory;
using System.Net;

namespace QuickBaseApi.Tests
{
    public class EditRecordTests : BaseTest
    {
        [Test]
        public async Task EditRecord_AllFields_ShouldSucceed()
        {
            var originalRecord = GenerateValidRecordForTable(TasksFields, RequiredFieldFilter.All);            
            var createRequestBody = RecordFactory.CreateRecord(TasksTable.Id, originalRecord);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequestBody);
            var createResponseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content);
            var recordId = createResponseContent.Metadata.CreatedRecordIds.First().ToString();

            var updatedRecord = GenerateValidRecordForTable(TasksFields, RequiredFieldFilter.All);
            updatedRecord["3"] = new FieldValueModel { value = recordId };
            var fieldsToReturn = HelperMethods.GetAllFieldIds(updatedRecord);
            var updateRequestBody = RecordFactory.CreateRecord(TasksTable.Id, updatedRecord, fieldsToReturn.ToArray());

            var editResponse = await QuickBaseClient.PostRecordAsync(updateRequestBody);
            var editResponseContent = JsonConvert.DeserializeObject<EditRecordResponseModel>(editResponse.Content);

            Assert.Multiple(() =>
            {
                Assert.That(editResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(editResponseContent.Metadata.UpdatedRecordIds.FirstOrDefault(), Is.EqualTo(int.Parse(recordId)));
                Assert.That(editResponseContent.Metadata.TotalNumberOfRecordsProcessed, Is.EqualTo(1));
            });

            Assertions.AssertFieldValues(editResponseContent.Data, new List<Dictionary<string, FieldValueModel>> { updatedRecord }, fieldsToReturn);
        }
    }
}
