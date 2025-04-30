using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Interfaces;
public interface IDatabaseService
{
    /// <summary>
    /// Adds an alert to the database.
    /// </summary>
    /// <param name="alert"></param>
    /// <returns>Nothing</returns>
    Task AddAlertAsync(Alert alert);

    /// <summary>
    /// Removes an alert from the database.
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns>Nothing</returns>
    Task RemoveAlertAsync(Guid alertId);

    /// <summary>
    /// Gets all alerts from the database.
    /// </summary>
    /// <returns>List of <see cref="Alert"/></returns>
    Task<List<Alert>> GetAlertsAsync();
}