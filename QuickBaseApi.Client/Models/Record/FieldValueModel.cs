using Newtonsoft.Json;
using QuickBaseApi.Client.Utils;

namespace QuickBaseApi.Client.Models
{
    public class FieldValueModel
    {
        [JsonProperty("value")]
        [JsonConverter(typeof(FieldValueConverter))]
        public object value { get; set; }
    }
}
