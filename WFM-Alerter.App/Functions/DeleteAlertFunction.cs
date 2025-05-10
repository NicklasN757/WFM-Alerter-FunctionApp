using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using WFM_Alerter.Service.Interfaces;

namespace WFM_Alerter.App.Functions;
internal class DeleteAlertFunction(ILoggerFactory loggerFactory, IDatabaseService databaseService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DeleteAlertFunction>();
    private readonly IDatabaseService _databaseService = databaseService;

    [Function("DeleteAlert")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "deletealert/{alertId}")] HttpRequestData req, FunctionContext executionContext, string alertId)
    {
        _logger.LogInformation("DeleteAlertFunction started with alertId: {AlertId}", alertId);
        Guid alertGuid = Guid.Parse(alertId);
        HttpStatusCode statusCode = await _databaseService.RemoveAlertAsync(alertGuid);
        return req.CreateResponse(statusCode); ;
    }
}