using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class RecordModel
    {
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public string? To { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Dictionary<string, FieldValueModel>> Data { get; set; }

        [JsonProperty("fieldsToReturn", NullValueHandling = NullValueHandling.Ignore)]
        public long[] FieldsToReturn { get; set; }
    }
}