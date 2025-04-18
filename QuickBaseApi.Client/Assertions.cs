﻿using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Text.RegularExpressions;
using QuickBaseApi.Client.Models;

namespace QuickBaseApi.Client
{
    public static class Assertions
    {
        public static void AssertFieldValues(Dictionary<string, FieldValueModel>[] actualData, List<Dictionary<string, FieldValueModel>> expectedRecords, List<long> fieldsToReturn)
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < actualData.Length; i++)
                {
                    var actualRecord = actualData[i];
                    var expectedRecord = expectedRecords[i];

                    Assert.Multiple(() =>
                    {
                        foreach (var fieldId in fieldsToReturn)
                        {
                            var key = fieldId.ToString();

                            Assert.That(actualRecord.ContainsKey(key), Is.True, $"Field '{key}' missing in actual record {i}.");
                            Assert.That(expectedRecord.ContainsKey(key), Is.True, $"Field '{key}' missing in expected record {i}.");

                            var actualValue = actualRecord[key]?.Value;
                            var expectedValue = expectedRecord[key]?.Value;

                            var actualNormalized = NormalizeValue(actualValue);
                            var expectedNormalized = NormalizeValue(expectedValue);

                            Assert.That(actualNormalized, Is.EqualTo(expectedNormalized),
                                $"Mismatch in record {i}, field '{key}': expected '{expectedNormalized}', got '{actualNormalized}'.");
                        }
                    });
                }
            });
        }

        private static string NormalizeValue(object value)
        {
            if (value is JObject jObject)
            {
                if (jObject.ContainsKey("id"))
                    return jObject["id"]?.ToString();

                if (jObject.ContainsKey("name"))
                    return ExtractIdFromAngleBrackets(jObject["name"]?.ToString());
            }

            if (value is IDictionary<string, object> dictionaryObject)
            {
                if (dictionaryObject.TryGetValue("id", out var id))
                    return id?.ToString();

                if (dictionaryObject.TryGetValue("name", out var name))
                    return ExtractIdFromAngleBrackets(name?.ToString());
            }

            return value?.ToString();
        }

        private static string ExtractIdFromAngleBrackets(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            var match = Regex.Match(input, "<(.*?)>");
            return match.Success ? match.Groups[1].Value : input;
        }

        public static void AssertRequiredFieldsMissingErrorMessage(CreateRecordResponseModel response, List<FieldModel> fields)
        {
            var requiredFieldIds = fields
                .Where(f => f.Required == true)
                .Select(f => f.Id.ToString())
                .ToHashSet();

            foreach (var lineError in response.Metadata.LineErrors)
            {
                string lineNumber = lineError.Key;
                string[] errorMessages = lineError.Value;

                foreach (var errorMessage in errorMessages)
                {
                    bool containsRequiredId = requiredFieldIds.Any(id => errorMessage.Contains($"Missing value for required field with ID \"{id}\""));

                    if (!containsRequiredId)
                    {
                        throw new Exception($"Error message on line {lineNumber} does not match any required field ID: {errorMessage}");
                    }
                }
            }
        }
    }
}
