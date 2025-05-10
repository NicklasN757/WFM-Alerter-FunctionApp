# Functions summary
Short description of the functions in this project.

## GetAlerts
HTTP trigger to get alerts from the database. Returns a list of alerts in JSON format.

## DeleteAlert
HTTP trigger to delete an alert from the database. Expects an alert ID in the request URL.

## AddAlert
HTTP trigger to add alerts to the database. Expects a JSON payload with the alert details in the request body.

## TimedChecker
Timed trigger that checks the database for alerts that need to be sent. If any alerts are found, compares them with the current pricing data and sends notifications if necessary. The function runs every 5 minutes. 