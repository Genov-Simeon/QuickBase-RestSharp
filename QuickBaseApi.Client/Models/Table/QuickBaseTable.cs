using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class QuickBaseTable
    {
        [JsonProperty("alias", NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty("created", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Created { get; set; }

        [JsonProperty("defaultSortFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int DefaultSortFieldId { get; set; }

        [JsonProperty("defaultSortOrder", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultSortOrder { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("keyFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int KeyFieldId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("nextFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int NextFieldId { get; set; }

        [JsonProperty("nextRecordId", NullValueHandling = NullValueHandling.Ignore)]
        public int NextRecordId { get; set; }

        [JsonProperty("pluralRecordName", NullValueHandling = NullValueHandling.Ignore)]
        public string PluralRecordName { get; set; }

        [JsonProperty("singleRecordName", NullValueHandling = NullValueHandling.Ignore)]
        public string SingleRecordName { get; set; }

        [JsonProperty("sizeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public string SizeLimit { get; set; }

        [JsonProperty("spaceRemaining", NullValueHandling = NullValueHandling.Ignore)]
        public string SpaceRemaining { get; set; }

        [JsonProperty("spaceUsed", NullValueHandling = NullValueHandling.Ignore)]
        public string SpaceUsed { get; set; }

        [JsonProperty("updated", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Updated { get; set; }
    }
}