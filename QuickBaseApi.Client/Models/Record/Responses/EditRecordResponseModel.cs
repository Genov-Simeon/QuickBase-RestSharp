using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class EditRecordResponseModel
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public List<Dictionary<string, FieldValueModel>> Data { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Metadata Metadata { get; set; }
    }
}