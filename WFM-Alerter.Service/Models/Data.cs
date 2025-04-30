using System.Text.Json.Serialization;

namespace WFM_Alerter.Service.Models;
public class Data
{
    [JsonPropertyName("sell")]
    public required List<ItemListing?> SellOrders { get; set; }

    [JsonPropertyName("buy")]
    public required List<ItemListing?> BuyOrders { get; set; }
}