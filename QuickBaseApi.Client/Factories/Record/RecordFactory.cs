using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Factories
{
    public static class RecordFactory
    {
        public static RecordModel Build(string tableId, List<Dictionary<string, FieldValueModel>> records, long[] fieldsToReturn = null)
        {
            return new RecordModel
            {
                To = tableId,
                Data = records,
                FieldsToReturn = fieldsToReturn
            };
        }
    }
}
