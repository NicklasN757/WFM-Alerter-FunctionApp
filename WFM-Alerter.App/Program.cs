using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WFM_Alerter.Service.Domain;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

const string DbPath = "./alerts.db";
const string Azure_DbPath = "D:/home/alerts.db";

bool isDevEnv = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development";

if (!isDevEnv && !File.Exists(Azure_DbPath))
{
    File.Copy(DbPath,Azure_DbPath);
    File.SetAttributes(Azure_DbPath, FileAttributes.Normal);
}

// Add configuration from appsettings or local.settings.json
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("host.json", optional: false, reloadOnChange: true)
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Configuration
    .AddJsonFile("host.json", optional: true, reloadOnChange: true)
    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);

// Register HttpClient for ApiService
builder.Services.AddHttpClient("WFMApiClient", client =>
    client.BaseAddress = configuration.GetValue<Uri>("WfmApi:BaseUrl"));

// Register DbContext with SQLite connection string
builder.Services.AddDbContext<ApplicationDbContext>( options =>
{
    if (!isDevEnv)
    {
        options.UseSqlite($"Data Source={Azure_DbPath}");
    }
    else
    {
        options.UseSqlite($"Data Source={DbPath}");
    }
    //options.UseSqlite(DatabaseService.GetSQLiteConnectionString());
});

// Register Application Insights telemetry
builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Register services
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

builder.Services.AddLogging();
builder.Build().Run();