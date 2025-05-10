# Missing files
This project is missing some files. The following files are missing, but are required for the project to run, these were removed from the repository for security reasons, and to avoid exposing sensitive information.

## host.json
Needed for Azure Functions to run. This file is used to configure the Azure Functions host. It contains settings that control the behavior of the function app, such as logging, timeouts, and other runtime settings.

```json
{
  "version": "2.0",
  "logging": {
    "applicationInsights": {
      "samplingSettings": {
        "isEnabled": true,
        "excludedTypes": "Request"
      },
      "enableLiveMetricsFilters": true
    }
  },
  "WfmApi": {
    "BaseUrl": "https://api.warframe.market/v2/" // Base URL for the Warframe Market API
  },
  "KeyVault": {
    "BaseUrl": "" // Key Vault URL
  },
  "EmailServiceConfiguration": {
    "SenderAddress": "" // Email sender address
  }
}
```

## local.settings.json
Needed for Azure functions to run locally, and for debugging purposes. This file contains settings that are used when running the function app locally, such as application settings, and other configuration values. It is not used in production and should not be deployed to Azure.

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```