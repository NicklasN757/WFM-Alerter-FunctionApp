using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using WFM_Alerter.Service.Interfaces;

namespace WFM_Alerter.App.Functions;
internal class DeleteAlertFunction
{
    private readonly ILogger _logger;
    private readonly IDatabaseService _databaseService;
    public DeleteAlertFunction(ILoggerFactory loggerFactory, IDatabaseService databaseService)
    {
        _logger = loggerFactory.CreateLogger<AddAlertFunction>();
        _databaseService = databaseService;
    }

    [Function("DeleteAlert")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deletealert/{alertId}")] HttpRequestData req, FunctionContext executionContext, string alertId)
    {
        _logger.LogInformation("DeleteAlertFunction started with alertId: {AlertId}", alertId);
        Guid alertGuid = Guid.Parse(alertId);
        await _databaseService.RemoveAlertAsync(alertGuid);
        HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        return response;
    }
}