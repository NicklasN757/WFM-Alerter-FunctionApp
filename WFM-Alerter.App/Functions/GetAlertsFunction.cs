using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.App.Functions;
internal class GetAlertsFunction(ILoggerFactory loggerFactory, IDatabaseService databaseService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetAlertsFunction>();
    private readonly IDatabaseService _databaseService = databaseService;

    [Function("GetAlerts")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getalerts")] HttpRequestData req, FunctionContext executionContext)
    {
        _logger.LogInformation("GetAlertsFunction started.");
        try
        {
            List<Alert> alerts = await _databaseService.GetAlertsAsync();
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(alerts);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get alerts: {ErrorMeassage}", ex.Message);
            return req.CreateResponse(HttpStatusCode.InternalServerError);
        }
    }
}