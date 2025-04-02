using System.Text.Json.Serialization;

public class DeleteRecordResponseModel
{
    [JsonPropertyName("numberDeleted")]
    public int NumberDeleted { get; set; }
}
