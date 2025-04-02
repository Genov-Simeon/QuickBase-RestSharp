using QuickBaseApi.Client.Models;
using static QuickBaseApi.Client.Factories.FieldFactory;

namespace QuickBaseApi.Client.Factories
{
    public static class CreateRecordFactory
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

        public static List<Dictionary<string, FieldValueModel>> GenerateRecords(List<FieldModel> fields, int maxEntries = 5)
        {
            var random = new Random();
            int count = random.Next(1, maxEntries + 1);

            var records = new List<Dictionary<string, FieldValueModel>>();

            for (int i = 0; i < count; i++)
            {
                var record = GenerateRecord(fields);
                records.Add(record);
            }

            return records;
        }


        public static Dictionary<string, FieldValueModel> GenerateRecord(List<FieldModel> fields, Func<FieldModel, bool> predicate = null)
        {
            var record = new Dictionary<string, FieldValueModel>();

            foreach (var field in fields)
            {
                if (predicate != null && !predicate(field))
                    continue;

                var value = GenerateRandomValueForField(field);

                if (value != null)
                {
                    record[field.Id.ToString()] = new FieldValueModel { Value = value };
                }
            }

            return record;
        }
    }
}
