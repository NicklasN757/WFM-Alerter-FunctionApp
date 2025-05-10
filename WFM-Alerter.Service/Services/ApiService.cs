using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Services;
public class ApiService(ILoggerFactory loggerFactory, IHttpClientFactory httpClientFactory) : IApiService
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<ApiService>();
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("WFMApiClient");

    public async Task<ApiResponse> GetApiResponseAsync(Alert alertItem) 
    {
        StringBuilder requestUri = new($"orders/item/{alertItem.ItemName}/top");
        if (alertItem.ItemRank != 0)
        {
            requestUri.Append($"?rank={alertItem.ItemRank}");
        }

        try
        {
            // Get the top orders for the giving item
            _logger.LogInformation("Fetching data from the API with base address {BaseAddress} for item: {ItemId}.", _httpClient.BaseAddress , alertItem.ItemName);
            ApiResponse apiResponse = await _httpClient.GetFromJsonAsync<ApiResponse>(requestUri.ToString());
            _logger.LogInformation("Data fetched successfully for item: {ItemId}.", alertItem.ItemName);

            return apiResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data from the API.");
            return null;
        }
    }
}