using Newtonsoft.Json;

namespace QuickBaseApi.Client.Models
{
    public class QuickBaseFieldPropertiesModel
    {
        [JsonProperty("allowHTML", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowHtml { get; set; }

        [JsonProperty("allowMentions", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowMentions { get; set; }

        [JsonProperty("allowNewChoices", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowNewChoices { get; set; }

        [JsonProperty("appendOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AppendOnly { get; set; }

        [JsonProperty("carryChoices", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CarryChoices { get; set; }

        [JsonProperty("defaultValue", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultValue { get; set; }

        [JsonProperty("foreignKey", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ForeignKey { get; set; }

        [JsonProperty("formula", NullValueHandling = NullValueHandling.Ignore)]
        public string Formula { get; set; }

        [JsonProperty("maxLength", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxLength { get; set; }

        [JsonProperty("numLines", NullValueHandling = NullValueHandling.Ignore)]
        public int? NumLines { get; set; }

        [JsonProperty("primaryKey", NullValueHandling = NullValueHandling.Ignore)]
        public bool? PrimaryKey { get; set; }

        [JsonProperty("sortAsGiven", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SortAsGiven { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public int? Width { get; set; }

        [JsonProperty("choices", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Choices { get; set; }

        [JsonProperty("displayUser", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayUser { get; set; }

        [JsonProperty("summaryFunction", NullValueHandling = NullValueHandling.Ignore)]
        public string SummaryFunction { get; set; }

        [JsonProperty("summaryReferenceFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int? SummaryReferenceFieldId { get; set; }

        [JsonProperty("summaryTargetFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int? SummaryTargetFieldId { get; set; }

        [JsonProperty("numberFormat", NullValueHandling = NullValueHandling.Ignore)]
        public int? NumberFormat { get; set; }

        [JsonProperty("useNewWindow", NullValueHandling = NullValueHandling.Ignore)]
        public bool? UseNewWindow { get; set; }

        [JsonProperty("linkText", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkText { get; set; }

        [JsonProperty("targetTableId", NullValueHandling = NullValueHandling.Ignore)]
        public string TargetTableId { get; set; }

        [JsonProperty("sourceFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int? SourceFieldId { get; set; }

        [JsonProperty("targetFieldId", NullValueHandling = NullValueHandling.Ignore)]
        public int? TargetFieldId { get; set; }

        [JsonProperty("versionMode", NullValueHandling = NullValueHandling.Ignore)]
        public string VersionMode { get; set; }

        [JsonProperty("seeVersions", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SeeVersions { get; set; }

        [JsonProperty("maxVersions", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxVersions { get; set; }
    }
}
