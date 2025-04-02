using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class CreateRecordResponseModel
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, FieldValueModel>[]? Data { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Metadata? Metadata { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("createdRecordIds", NullValueHandling = NullValueHandling.Ignore)]
        public long[]? CreatedRecordIds { get; set; }

        [JsonProperty("lineErrors", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string[]>? LineErrors { get; set; }

        [JsonProperty("totalNumberOfRecordsProcessed", NullValueHandling = NullValueHandling.Ignore)]
        public long TotalNumberOfRecordsProcessed { get; set; }

        [JsonProperty("unchangedRecordIds", NullValueHandling = NullValueHandling.Ignore)]
        public long[]? UnchangedRecordIds { get; set; }

        [JsonProperty("updatedRecordIds", NullValueHandling = NullValueHandling.Ignore)]
        public long[]? UpdatedRecordIds { get; set; }
    }
}