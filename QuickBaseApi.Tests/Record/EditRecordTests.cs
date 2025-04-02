using Newtonsoft.Json;
using NUnit.Framework;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Utils;
using QuickBaseApi.Client;
using static QuickBaseApi.Client.Factories.CreateRecordFactory;
using System.Net;
using QuickBaseApi.Client.Enums;

namespace QuickBaseApi.Tests
{
    public class EditRecordTests : BaseTest
    {
        [Test]
        public async Task EditRecord_AllFields_ShouldSucceed()
        {
            var originalRecord = GenerateRecord(TasksFields);            
            var createRequestBody = CreateRecord(TasksTable.Id, originalRecord);
            var createResponse = await QuickBaseClient.PostRecordAsync(createRequestBody);
            var createResponseContent = JsonConvert.DeserializeObject<CreateRecordResponseModel>(createResponse.Content);
            var recordId = createResponseContent.Metadata.CreatedRecordIds.First().ToString();

            var updatedRecord = GenerateRecord(TasksFields);
            updatedRecord["3"] = new FieldValueModel { Value = recordId };
            var fieldsToReturn = HelperMethods.GetAllFieldIds(updatedRecord);
            var updateRequestBody = CreateRecord(TasksTable.Id, updatedRecord, fieldsToReturn.ToArray());

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
