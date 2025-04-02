using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Factories
{
    public static class RecordFactory
    {
        public static RecordModel BuildSingleRecord(string tableId, Dictionary<string, FieldValueModel> record, long[] fieldsToReturn = null)
        {
            var records = new List<Dictionary<string, FieldValueModel>> { record };

            var model = new RecordModel
            {
                To = tableId,
                Data = records
            };

            if (fieldsToReturn == null)
            {
                model.FieldsToReturn = Array.Empty<long>();
            }
            else
            {
                model.FieldsToReturn = fieldsToReturn;
            }

            return model;
        }

        public static RecordModel BuildMultipleRecords(string tableId, List<Dictionary<string, FieldValueModel>> records, long[] fieldsToReturn = null)
        {
            var model = new RecordModel
            {
                To = tableId,
                Data = records
            };

            if (fieldsToReturn == null)
            {
                model.FieldsToReturn = Array.Empty<long>();
            }
            else
            {
                model.FieldsToReturn = fieldsToReturn;
            }

            return model;
        }
    }
}
