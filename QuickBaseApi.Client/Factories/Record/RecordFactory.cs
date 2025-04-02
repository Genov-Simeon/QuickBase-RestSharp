using QuickBaseApi.Client.Enums;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Factories
{
    public static class RecordFactory
    {
        public static CreateRecordModel CreateRecord(string tableId, Dictionary<string, FieldValueModel> record, long[] fieldsToReturn = null)
        {
            var records = new List<Dictionary<string, FieldValueModel>> { record };

            return new CreateRecordModel
            {
                To = tableId,
                Data = records,
                FieldsToReturn = fieldsToReturn ?? Array.Empty<long>()
            };
        }

        public static CreateRecordModel CreateRecords(string tableId, List<Dictionary<string, FieldValueModel>> records, long[] fieldsToReturn = null)
        {
            return new CreateRecordModel
            {
                To = tableId,
                Data = records,
                FieldsToReturn = fieldsToReturn ?? Array.Empty<long>()
            };
        }

        public static DeleteRecordModel DeleteByQuery(string tableId, string value, QueryOperator op = QueryOperator.EX, string fieldId = "3")
        {
            string whereClause = $"{{{fieldId}.{op}.{value}}}";

            return new DeleteRecordModel
            {
                From = tableId,
                Where = whereClause
            };
        }
    }
}
