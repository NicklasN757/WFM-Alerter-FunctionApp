using System.Net;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Interfaces;
public interface IDatabaseService
{
    /// <summary>
    /// Adds an alert to the database.
    /// </summary>
    /// <param name="alerts"></param>
    /// <returns>Nothing</returns>
    Task<HttpStatusCode> AddAlertAsync(List<Alert> alerts);

    /// <summary>
    /// Removes an alert from the database.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns>Nothing</returns>
    Task<HttpStatusCode> RemoveAlertAsync(Guid alertId);

    /// <summary>
    /// Gets all alerts from the database.
    /// </summary>
    /// <returns>List of <see cref="Alert"/></returns>
    Task<List<Alert>> GetAlertsAsync();
}