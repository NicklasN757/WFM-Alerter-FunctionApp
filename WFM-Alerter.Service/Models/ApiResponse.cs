using System.Text.Json.Serialization;

namespace WFM_Alerter.Service.Models;
public class ApiResponse
{
    [JsonPropertyName("apiVersion")]
    public required string ApiVersion { get; set; }

    [JsonPropertyName("data")]
    public Data Data { get; set; }
}