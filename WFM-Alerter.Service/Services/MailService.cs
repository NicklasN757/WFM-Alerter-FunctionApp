using Azure;
using Azure.Communication.Email;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using WFM_Alerter.Service.Interfaces;
using WFM_Alerter.Service.Models;

namespace WFM_Alerter.Service.Services;
public class MailService : IMailService
{
	private readonly ILogger _logger;
	private readonly IConfiguration _configuration;
	private readonly SecretClient _secretClient;
	public MailService(ILoggerFactory loggerFactory, IConfiguration configuration)
	{
		_logger = loggerFactory.CreateLogger<MailService>();
		_configuration = configuration;
		_secretClient = new(new Uri(_configuration["KeyVault:BaseUrl"]!), new DefaultAzureCredential());
	}

	// Sends an email to the user when the alert is triggered.
	public async Task<bool> SendEmailAsync(Alert alert, ItemListing itemListing)
	{
		string friendlyItemName = alert.ItemName.Replace("_", " ");
		KeyVaultSecret secret = await _secretClient.GetSecretAsync("EmailConnectionString");

		// Create the ingame message
		StringBuilder ingameMessage = new($"/w {itemListing.Seller.IngameName} Hi! WTB {friendlyItemName}");
		if (alert.ItemRank != 0) 
		{
			ingameMessage.Append($" (Rank {alert.ItemRank})");
		}
		ingameMessage.Append($", {itemListing.PlatinumPrice}P.");

		try
		{
			EmailClient emailClient = new(secret.Value);

			// Create the email message
			EmailMessage emailMessage = new(
			senderAddress: _configuration["EmailServiceConfiguration:SenderAddress"],

			content: new EmailContent($"New WFM alert - {friendlyItemName}")
			{
				Html = @$"
							<html>
								<body>
									<h1>Item: {friendlyItemName} is now selling for {itemListing.PlatinumPrice} platnium.</h1>
									<p>Click <a href='https://warframe.market/items/{alert.ItemName}'>here</a> to view the item on the website.</p>
									<p>Ingame message:</p>
									<p>{ingameMessage}</p>
								</body>
							</html>
						",
			},
			recipients: new(new List<EmailAddress> { new(alert.Email) })); // List of recipients

			// Send the email
			_logger.LogInformation("Sending email to {Email} for item {ItemName} at price {Price}.", alert.Email, alert.ItemName, itemListing.PlatinumPrice);
			EmailSendOperation emailSendOperation = await emailClient.SendAsync(WaitUntil.Started, emailMessage);
			_logger.LogInformation("Email sent to {Email} for item {ItemName} at price {Price}.", alert.Email, alert.ItemName, itemListing.PlatinumPrice);

			return true;
		}
		catch (Exception ex)
		{
			// Log the error
			_logger.LogError(ex, "Error sending email to {Email} for item {ItemName}.", alert.Email, alert.ItemName);
			return false;
		}
	}
}