using System.Text.Json.Serialization;

namespace WFM_Alerter.Service.Models;
public class ItemListing
{
    [JsonPropertyName("platinum")]
    public required int PlatinumPrice { get; set; }

    [JsonPropertyName("quantity")]
    public required int Quantity { get; set; }

    [JsonPropertyName("rank")]
    public int? ItemRank { get; set; }

    [JsonPropertyName("user")]
    public required MarketUser Seller { get; set; }
}