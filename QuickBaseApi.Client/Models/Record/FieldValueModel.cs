using Newtonsoft.Json;
using QuickBaseApi.Client.Utils;

namespace QuickBaseApi.Client.Models
{
    public class FieldValueModel
    {
        [JsonProperty("value")]
        public object value { get; set; }
    }
}
