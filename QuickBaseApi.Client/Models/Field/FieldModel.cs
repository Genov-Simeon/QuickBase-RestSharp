using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class FieldModel
    {
        [JsonProperty("appearsByDefault", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AppearsByDefault { get; set; }

        [JsonProperty("audited", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Audited { get; set; }

        [JsonProperty("bold", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Bold { get; set; }

        [JsonProperty("doesDataCopy", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DoesDataCopy { get; set; }

        [JsonProperty("fieldHelp", NullValueHandling = NullValueHandling.Ignore)]
        public string? FieldHelp { get; set; }

        [JsonProperty("fieldType", NullValueHandling = NullValueHandling.Ignore)]
        public string? FieldType { get; set; }

        [JsonProperty("findEnabled", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FindEnabled { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string? Label { get; set; }

        [JsonProperty("mode", NullValueHandling = NullValueHandling.Ignore)]
        public string? Mode { get; set; }

        [JsonProperty("noWrap", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NoWrap { get; set; }

        [JsonProperty("properties", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object>? QuickBaseFieldProperties { get; set; }

        [JsonProperty("required", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Required { get; set; }

        [JsonProperty("unique", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Unique { get; set; }
    }
}
