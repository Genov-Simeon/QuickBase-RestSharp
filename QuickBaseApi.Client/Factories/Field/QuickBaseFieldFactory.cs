using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client.Factories
{
    public static class QuickBaseFieldFactory
    {
        public enum RequiredFieldFilter
        {
            All,            
            OnlyRequired,   
            OnlyOptional
        }

        public static List<Dictionary<string, FieldValueModel>> GenerateValidRecordsForTable(List<QuickBaseFieldModel> fields, RequiredFieldFilter filter = RequiredFieldFilter.All, int maxEntries = 5)
        {
            var random = new Random();
            int count = random.Next(1, maxEntries + 1);

            var records = new List<Dictionary<string, FieldValueModel>>();

            for (int i = 0; i < count; i++)
            {
                var record = GenerateValidRecordForTable(fields, filter);
                records.Add(record);
            }

            return records;
        }


        public static Dictionary<string, FieldValueModel> GenerateValidRecordForTable(List<QuickBaseFieldModel> fields, RequiredFieldFilter filter = RequiredFieldFilter.All)
        {
            var record = new Dictionary<string, FieldValueModel>();

            foreach (var field in fields)
            {
                if (!IsWritableField(field))
                    continue;

                if (filter == RequiredFieldFilter.OnlyRequired && field.Required != true)
                    continue;

                if (filter == RequiredFieldFilter.OnlyOptional && field.Required == true)
                    continue;

                var value = GenerateRandomValueForField(field);

                if (value != null)
                {
                    record[field.Id.ToString()] = new FieldValueModel { value = value };
                }
            }

            return record;
        }

        public static List<(Dictionary<string, FieldValueModel> RequiredRecord, Dictionary<string, FieldValueModel> InvalidRecord)> GenerateInValidRecordsForTable(List<QuickBaseFieldModel> fields, int maxEntries = 5)
        {
            var random = new Random();
            int count = random.Next(1, maxEntries + 1);

            var records = new List<(Dictionary<string, FieldValueModel> RequiredRecord, Dictionary<string, FieldValueModel> InvalidRecord)>();


            for (int i = 0; i < count; i++)
            {
                var record = GenerateInvalidRecordForTable(fields);
                records.Add(record);
            }

            return records;
        }

        // Add required fields and invalid data restricted to "numeric" and "work date" fields. Other fields not 
        public static ( Dictionary<string, FieldValueModel> RequiredRecord, Dictionary<string, FieldValueModel> InvalidRecord) GenerateInvalidRecordForTable( List<QuickBaseFieldModel> fields)
        {
            var requiredPart = new Dictionary<string, FieldValueModel>();
            var invalidPart = new Dictionary<string, FieldValueModel>();

            foreach (var field in fields)
            {
                if (field.Required == true)
                {
                    requiredPart[field.Id.ToString()] = new FieldValueModel
                    {
                        value = GenerateRandomValueForField(field)
                    };
                    continue;
                }

                if (field.FieldType?.ToLowerInvariant() is not "numeric" or "work date")
                    continue;

                invalidPart[field.Id.ToString()] = new FieldValueModel
                {
                    value = GenerateInvalidValueForField(field)
                };
            }

            return (requiredPart, invalidPart);
        }

        private static object GenerateRandomValueForField(QuickBaseFieldModel field)
        {
            var random = new Random();

            // Helper for extracting choices
            List<string> GetChoices()
            {
                if (field.QuickBaseFieldProperties != null &&
                    field.QuickBaseFieldProperties.TryGetValue("choices", out var choicesObj))
                {
                    if (choicesObj is JArray jArray)
                        return jArray.Select(c => c.ToString()).ToList();

                    return JsonConvert.DeserializeObject<List<string>>(choicesObj.ToString());
                }

                return null;
            }

            return field.FieldType?.ToLowerInvariant() switch
            {
                "text" => $"Random text {Guid.NewGuid():N}[..8]",

                "text-multi-line" => $"Multiline example:\nLine {random.Next(1, 100)}\nAnother line {Guid.NewGuid():N}[..5]",

                // extract choices from field properties or use default
                "text-multiple-choice" => GetChoices()?.OrderBy(_ => random.Next()).FirstOrDefault()
                                          ?? new[] { "Option1", "Option2", "Option3" }[random.Next(3)],

                // extract choices from field properties or use default
                "multitext" => GetChoices()?.OrderBy(_ => random.Next()).Take(random.Next(1, 3)).ToList()
                               ?? new List<string> { "Option A", "Option B" },

                "rich-text" => $"<b>Rich content {Guid.NewGuid():N}[..4]</b><br><i>Generated at {DateTime.UtcNow:HH:mm:ss}</i>",

                "email" => $"user{random.Next(1000, 9999)}@example.com",

                "url" => $"https://example.com/page/{Guid.NewGuid():N}[..5]",

                "numeric" => random.Next(0, 1000),

                "percent" => Math.Round(random.NextDouble(), 2),

                "rating" => random.Next(1, 6),

                "currency" => Math.Round(random.NextDouble() * 10000, 2),

                "duration" => random.Next(1, 365) * 86400000, // days in ms

                "date" => DateTime.UtcNow.AddDays(random.Next(-365, 365)).ToString("yyyy-MM-dd"),

                "datetime" => DateTime.UtcNow.AddMinutes(random.Next(-10000, 10000))
                                   .ToString("yyyy-MM-ddTHH:mm:ssZ"),

                "timeofday" => new TimeSpan(random.Next(0, 24), random.Next(0, 60), 0)
                                   .ToString(@"hh\:mm\:ss"),

                "checkbox" => random.NextDouble() > 0.5,

                "phone" => $"({random.Next(100, 999)}) {random.Next(100, 999)}-{random.Next(1000, 9999)}",

                "user" => new { id = $"{random.Next(100000, 999999)}.{Guid.NewGuid():N}[..4]" },

                "multiuser" => Enumerable.Range(0, random.Next(1, 3))
                                        .Select(_ => new { id = $"{random.Next(100000, 999999)}.{Guid.NewGuid():N}[..4]" })
                                        .ToList(),
                _ => null
            };
        }

        private static object GenerateInvalidValueForField(QuickBaseFieldModel field)
        {
            return field.FieldType?.ToLowerInvariant() switch
            {
                "numeric" => "not-a-number",

                "date" => "31-02-2025",

                _ => null
            };
        }

        public static bool IsWritableField(QuickBaseFieldModel field)
        {
            var fieldType = field.FieldType?.ToLowerInvariant();
            var label = field.Label?.ToLowerInvariant();

            // Exclude known system field types
            if (fieldType is "recordid" or "timestamp")
                return false;

            // Exclude user fields unless you explicitly handle real user mapping
            if (fieldType is "user" || fieldType is "multiuser")
                return false;

            // Exclude read-only fields
            if (fieldType is "ICalendarButton")
                return false;

            // Exclude known system labels; "Order" type field - Quickbase automatically reassigns new value to maintain internal sort order
            if (label is "date created" or "date modified" or "record id#" or "record owner" or "last modified by" or "assigned to" or "order")
                return false;

            // Exclude primary key fields
            if (field.QuickBaseFieldProperties.TryGetValue("PrimaryKey", out var value) && value is bool isPrimaryKey && isPrimaryKey)
                return false;

            // Exclude fields based on read-only mode
            if (!string.IsNullOrEmpty(field.Mode) && (field.Mode is "lookup" || field.Mode is "formula" || field.Mode is "summary"))
                return false;

            return true;
        }
    }
}
