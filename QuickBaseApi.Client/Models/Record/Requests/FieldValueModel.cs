using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class FieldValueModel
    {
        [JsonProperty("value")]
        public object value { get; set; }
    }
}
