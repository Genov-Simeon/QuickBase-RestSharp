using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Factories;

namespace QuickBaseApi.Client.Utils
{
    public static class FieldHelper
    {
        public static List<FieldModel> GetWritableFields(List<FieldModel> fields)
        {
            return fields.Where(FieldFactory.IsWritableField).ToList();
        }

        public static FieldModel GetRandomField(List<FieldModel> fields, Func<FieldModel, bool> predicate = null)
        {
            var effectivePredicate = predicate ?? (_ => true);
            var filteredFields = fields.Where(effectivePredicate).ToList();

            if (!filteredFields.Any())
                throw new InvalidOperationException("No fields matched the given condition.");

            var random = new Random();
            return filteredFields[random.Next(filteredFields.Count)];
        }

        public static List<long> GetAllFieldIds(Dictionary<string, FieldValueModel> randomRecord)
        {
            var fieldsToReturn = new List<long>();

            foreach (var id in randomRecord.Keys)
            {
                fieldsToReturn.Add(long.Parse(id));
            }

            return fieldsToReturn;
        }
    }
}
