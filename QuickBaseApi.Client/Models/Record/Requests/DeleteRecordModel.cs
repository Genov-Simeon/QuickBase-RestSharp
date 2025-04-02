using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class DeleteRecordModel
    {
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public string From { get; set; }

        [JsonProperty("where", NullValueHandling = NullValueHandling.Ignore)]
        public string Where { get; set; }
    }
}
