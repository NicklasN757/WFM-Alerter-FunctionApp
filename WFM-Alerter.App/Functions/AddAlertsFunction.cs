using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.App.Functions;
internal class AddAlertsFunction(ILoggerFactory loggerFactory, IDatabaseService databaseService)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<AddAlertsFunction>();
    private readonly IDatabaseService _databaseService = databaseService;

    [Function("AddAlerts")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "addalerts")] HttpRequestData req, FunctionContext executionContext)
    {
        _logger.LogInformation("AddAlertsFunction started.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        List<Alert>? alerts = JsonSerializer.Deserialize<List<Alert>>(requestBody);
        if (alerts == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
        HttpStatusCode statusCode = await _databaseService.AddAlertAsync(alerts);
        return req.CreateResponse(statusCode);
    }
}