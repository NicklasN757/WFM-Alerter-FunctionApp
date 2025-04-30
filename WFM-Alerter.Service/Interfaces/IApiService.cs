using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Interfaces;
public interface IApiService
{
    /// <summary>
    /// Fetches the top orders for a given item from the API.
    /// </summary>
    /// <param name="alertItem"></param>
    /// <returns>The API response as a <see cref="ApiResponse"/> object</returns>
    Task<ApiResponse?> GetApiResponseAsync(Alert alertItem);
}