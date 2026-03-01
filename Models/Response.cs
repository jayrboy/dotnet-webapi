using System.Text.Json.Serialization;

namespace WebApi.Models.Entities;

public partial class Response
{
    public int Status_code { get; set; }
    public string? Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }
}
