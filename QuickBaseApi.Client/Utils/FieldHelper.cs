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

        public static string GetFieldId(List<QuickBaseFieldModel> fields, Func<QuickBaseFieldModel, bool> predicate)
        {
            return fields.FirstOrDefault(predicate)?.Id.ToString();
        }

        public static QuickBaseFieldModel GetFieldByLabel(List<QuickBaseFieldModel> fields, string label)
        { 
            return fields.FirstOrDefault(f => f.Label == label);
        }

        public static Dictionary<string, QuickBaseFieldModel> MapFieldsByLabel(List<QuickBaseFieldModel> fields)
        {
            return fields
                .Where(f => !string.IsNullOrWhiteSpace(f.Label))
                .ToDictionary(f => f.Label, f => f, StringComparer.OrdinalIgnoreCase);
        }
    }
}
