using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.App.Functions;
internal class AddAlertFunction
{
    private readonly ILogger _logger;
    private readonly IDatabaseService _databaseService;
    public AddAlertFunction(ILoggerFactory loggerFactory, IDatabaseService databaseService)
    {
        _logger = loggerFactory.CreateLogger<AddAlertFunction>();
        _databaseService = databaseService;
    }

    [Function("AddAlert")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "addalert")] HttpRequestData req, FunctionContext executionContext)
    {
        _logger.LogInformation("AddAlertFunction started.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        Alert alert = JsonSerializer.Deserialize<Alert>(requestBody);
        if (alert == null)
        {
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
        await _databaseService.AddAlertAsync(alert);
        return req.CreateResponse(HttpStatusCode.OK);
    }
}