using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Interfaces;
public interface IMailService
{
    /// <summary>
    /// Sends an email to the user when the alert is triggered.
    /// </summary>
    /// <param name="alert"></param>
    /// <param name="itemListing"></param>
    /// <returns>Nothing</returns>
    Task<bool> SendEmailAsync(Alert alert, ItemListing itemListing);
}