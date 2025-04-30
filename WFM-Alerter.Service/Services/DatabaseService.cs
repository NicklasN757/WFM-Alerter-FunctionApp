using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WFM_Alerter.Service.Domain;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Services;
public class DatabaseService : IDatabaseService
{
    private readonly ILogger _logger;
    private readonly ApplicationDbContext _dbContext;
    public DatabaseService(ILoggerFactory loggerFactory, ApplicationDbContext applicationDbContext)
    {
        _logger = loggerFactory.CreateLogger<DatabaseService>();
        _dbContext = applicationDbContext;
    }

    // Get the SQLite connection string
    public static string GetSQLiteConnectionString() 
    {
        string home = Environment.GetEnvironmentVariable("HOME") ?? "";
        if (!string.IsNullOrEmpty(home))
        {
            home = Path.Combine(home, "site", "wwwroot");
        }
        string databasePath = Path.Combine(home, "alerts.db");
        string connStr = $"Data Source={databasePath}";

        return connStr;
    }

    // Add an alert to the database
    public async Task AddAlertAsync(Alert alert)
    {
        try
        {
            await _dbContext.AddAsync(alert);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add alert to the database.");
        }
        finally
        {
            await _dbContext.Database.CloseConnectionAsync();
            _logger.LogInformation("Database connection closed.");
        }
    }

    // Remove an alert from the database
    public async Task RemoveAlertAsync(Guid alertId)
    {
        try
        {
            Alert? alert = await _dbContext.Alerts.FindAsync(alertId);
            if (alert != null)
            {
                _dbContext.Remove(alert);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Alert with ID {AlertId} not found.", alertId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove alert from the database.");
        }
        finally
        {
            await _dbContext.Database.CloseConnectionAsync();
            _logger.LogInformation("Database connection closed.");
        }

    }

    // Get all alerts from the database
    public async Task<List<Alert>> GetAlertsAsync()
    {
        try
        {
            List<Alert> alerts = await _dbContext.Alerts.ToListAsync();
            if (alerts == null || alerts.Count == 0)
            {
                _logger.LogWarning("No alerts found in the database.");
                return new();
            }
            _logger.LogInformation("Retrieved {Count} alerts from the database.", alerts.Count);
            return alerts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get alerts from the database.");
            return new();
        }
        finally
        {
            await _dbContext.Database.CloseConnectionAsync();
            _logger.LogInformation("Database connection closed.");
        }
    }
}