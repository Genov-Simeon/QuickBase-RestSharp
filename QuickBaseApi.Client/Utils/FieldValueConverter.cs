using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace QuickBaseApi.Client.Utils
{
    public class FieldValueConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(object);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Use JToken to parse and return native types
            var token = JToken.Load(reader);

            return token.Type switch
            {
                JTokenType.Integer => token.ToObject<long>(),
                JTokenType.Float => token.ToObject<double>(),
                JTokenType.Boolean => token.ToObject<bool>(),
                JTokenType.String => token.ToObject<string>(),
                JTokenType.Date => token.ToObject<DateTime>(),
                _ => token.ToString()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Default serialization
            serializer.Serialize(writer, value);
        }
    }
}
