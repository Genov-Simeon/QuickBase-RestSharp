using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using QuickBaseApi.Client.Models;
using QuickBaseApi.Client.Utils;
using NUnit.Framework.Internal;

namespace QuickBaseApi.Client.Factories
{
    public static class FieldFactory
    {
        public static Dictionary<string, FieldValueModel> GenerateRandomFieldValue(List<FieldModel> fields)
        {
            var optionalField = FieldHelper.GetRandomField(fields);
            var value = GenerateRandomValueForField(optionalField);

            return new Dictionary<string, FieldValueModel>
            {
                [optionalField.Id.ToString()] = new FieldValueModel { Value = value }
            };
        }

        public static object GenerateRandomValueForField(FieldModel field)
        {
            var random = new Random();

            return field.FieldType?.ToLowerInvariant() switch
            {
                "text" => $"Random text {Guid.NewGuid():N}[..8]",

                "text-multi-line" => $"Multiline example:\nLine {random.Next(1, 100)}\nAnother line {Guid.NewGuid():N}[..5]",

                "text-multiple-choice" => GetChoices(field)?.OrderBy(_ => random.Next()).FirstOrDefault() ?? new[] { "Option1", "Option2", "Option3" }[random.Next(3)],

                "multitext" => GetChoices(field)?.OrderBy(_ => random.Next()).Take(random.Next(1, 3)).ToList() ?? new List<string> { "Option A", "Option B" },

                "rich-text" => $"<b>Rich content {Guid.NewGuid():N}[..4]</b><br><i>Generated at {DateTime.UtcNow:HH:mm:ss}</i>",

                "email" => $"user{random.Next(1000, 9999)}@example.com",

                "url" => $"https://example.com/page/{Guid.NewGuid():N}[..5]",

                "numeric" => random.Next(0, 1000),

                "percent" => Math.Round(random.NextDouble(), 2),

                "rating" => random.Next(1, 6),

                "currency" => Math.Round(random.NextDouble() * 10000, 2),

                "duration" => random.Next(1, 365) * 86400000, // days in ms

                "date" => DateTime.UtcNow.AddDays(random.Next(-365, 365)).ToString("yyyy-MM-dd"),

                "datetime" => DateTime.UtcNow.AddMinutes(random.Next(-10000, 10000)).ToString("yyyy-MM-ddTHH:mm:ssZ"),

                "timeofday" => new TimeSpan(random.Next(0, 24), random.Next(0, 60), 0).ToString(@"hh\:mm\:ss"),

                "checkbox" => random.NextDouble() > 0.5,

                "phone" => $"({random.Next(100, 999)}) {random.Next(100, 999)}-{random.Next(1000, 9999)}",

                "user" => new { id = $" User {random.Next(100000, 999999)}.{Guid.NewGuid():N}[..4]" },

                "multiuser" => Enumerable.Range(0, random.Next(1, 3)).Select(_ => new { id = $"{random.Next(100000, 999999)}.{Guid.NewGuid():N}[..4]" }).ToList(),
                
                _ => null
            };
        }

        private static List<string> GetChoices(FieldModel field)
        {
            if (field.Properties != null &&
                field.Properties.TryGetValue("choices", out var choices))
            {
                if (choices is JArray jArray)
                    return jArray.Select(c => c.ToString()).ToList();

                return JsonConvert.DeserializeObject<List<string>>(choices.ToString());
            }

            return null;
        }

        public static bool IsWritableField(FieldModel field)
        {
            var fieldType = field.FieldType?.ToLowerInvariant();
            var label = field.Label?.ToLowerInvariant();

            // These are system-managed fields that QuickBase automatically populates and maintains
            if (fieldType is "recordid" or "timestamp")
                return false;

            // Require special handling as they need valid QuickBase user IDs. Can be included if user ID mapping is implemented.
            if (fieldType is "user" || fieldType is "multiuser")
                return false;

            // Read-only interface element that can't be modified through the API
            if (fieldType is "icalendarbutton")
                return false;

            // System-maintained fields that are automatically managed by QuickBase.
            // "order" - Special field for maintaining record order (auto-reassigned by QuickBase)
            if (label is "date created" or "date modified" or "record id#" or "record owner" or "last modified by" or "assigned to" or "order")
                return false;

            // Primary keys are typically system-managed and need to maintain uniqueness. Attempting to modify could cause data integrity issues.
            if (field.Properties.TryGetValue("PrimaryKey", out var value) && value is bool isPrimaryKey && isPrimaryKey)
                return false;

            // Exclude append-only fields (cannot be edited)
            if (field.Properties.TryGetValue("appendOnly", out var appendOnlyObj) && appendOnlyObj is bool appendOnly && appendOnly)
                return false;

            // Computed automatically based on other fields or relationships.
            if (!string.IsNullOrEmpty(field.Mode) && (field.Mode is "lookup" || field.Mode is "formula" || field.Mode is "summary"))
                return false;

            return true;
        }
    }
}
