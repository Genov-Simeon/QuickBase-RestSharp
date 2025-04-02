using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Factories;

namespace QuickBaseApi.Client.Utils
{
    public static class FieldHelper
    {
        public static List<QuickBaseFieldModel> GetWritableFields(List<QuickBaseFieldModel> fields)
        {
            return fields
                .Where(FieldFactory.IsWritableField)
                .ToList();
        }

        public static string GetFieldIdByCondition(List<QuickBaseFieldModel> fields, Func<QuickBaseFieldModel, bool> predicate)
        {
            return fields.FirstOrDefault(predicate)?.Id.ToString();
        }


        public static QuickBaseFieldModel GetFieldByLabel(List<QuickBaseFieldModel> fields, string label)
        { 
            return fields.FirstOrDefault(f => f.Label == label);
        }

        public static QuickBaseFieldModel GetRandomField(List<QuickBaseFieldModel> fields, Func<QuickBaseFieldModel, bool> predicate = null)
        {
            var effectivePredicate = predicate ?? (_ => true);
            var filteredFields = fields.Where(effectivePredicate).ToList();

            if (!filteredFields.Any())
                throw new InvalidOperationException("No fields matched the given condition.");

            var random = new Random();
            return filteredFields[random.Next(filteredFields.Count)];
        }

        public static List<string> GetAllFieldLabels(List<QuickBaseFieldModel> fields)
        {
            return fields
                .Where(f => !string.IsNullOrWhiteSpace(f.Label))
                .Select(f => f.Label!)
                .ToList();
        }
    }
}
