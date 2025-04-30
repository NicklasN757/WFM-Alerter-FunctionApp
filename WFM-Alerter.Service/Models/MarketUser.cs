using System.Text.Json.Serialization;

namespace WFM_Alerter.Service.Models;
public class MarketUser
{
    [JsonPropertyName("ingameName")]
    public required string IngameName { get; set; }

    [JsonPropertyName("lastSeen")]
    public required DateTime LastSeen { get; set; }
}