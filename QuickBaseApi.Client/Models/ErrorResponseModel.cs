using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class ErrorResponseModel
    {
        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string? Message { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string? Description { get; set; }
    }
}
