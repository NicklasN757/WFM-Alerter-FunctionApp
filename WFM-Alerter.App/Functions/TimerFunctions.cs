using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.App.Functions;

public class TimerFunctions
{
    private readonly ILogger _logger;
    private readonly IMailService _emailService;
    private readonly IApiService _apiService;
    private readonly IDatabaseService _databaseService;

    public TimerFunctions(ILoggerFactory loggerFactory, IMailService mailService, IApiService apiService, IDatabaseService databaseService)
    {
        _logger = loggerFactory.CreateLogger<TimerFunctions>();
        _emailService = mailService;
        _apiService = apiService;
        _databaseService = databaseService;
    }

    [Function("TimedChecker")]
    public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo myTimer)
    {
        List<Alert> items = await _databaseService.GetAlertsAsync();

        foreach (Alert alertItem in items)
        {
            _logger.LogInformation("Checking for item: {ItemName} at price: {Price}.", alertItem.ItemName, alertItem.Price);

            ApiResponse response = await _apiService.GetApiResponseAsync(alertItem);
            if (response == null || response.Data.SellOrders.Count == 0)
            {
                _logger.LogWarning("No data found for item: {ItemName}.", alertItem.ItemName);
                continue;
            }

            ItemListing itemListing = response.Data.SellOrders[0]!;
            if (itemListing.PlatinumPrice <= alertItem.Price)
            {
                if (await _emailService.SendEmailAsync(alertItem, itemListing))
                {
                    _logger.LogInformation("Alert triggered for item: {ItemName} at price: {Price}.", alertItem.ItemName, itemListing.PlatinumPrice);

                    await _databaseService.RemoveAlertAsync(alertItem.Guid);
                    _logger.LogInformation("Alert removed for item: {ItemName}.", alertItem.ItemName);
                }
            }

            Thread.Sleep(500);
        }
    }
}