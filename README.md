# AzDataExplorerAndFunctions
A developer can explore many data streams with Azure Data Explorer. After a developer injest and explore the data, he/she want to query data from application layer. You can learn how to query data from Azure Data Explorer with .NET Core and Azure Functions.

## Prerequistics
- Have understood [What is Azure Data Explorer](https://docs.microsoft.com/en-us/azure/data-explorer/data-explorer-overview). This sample focuses on 3. Query database in the document.
- Have finished all [quick starts](https://docs.microsoft.com/en-us/azure/data-explorer/create-cluster-database-portal) in the document. This sample uses test datbase and table which you create in the tutorial.
- [Create Azure AD application](https://docs.microsoft.com/en-us/azure/active-directory/develop/howto-create-service-principal-portal) and have `client id`, `client secret` and `tenant id`.
- Installed Visual Studio Code
- Have finished [Visual Studio Code x Azure Functions tutorial](https://docs.microsoft.com/en-us/azure/azure-functions/functions-develop-vs-code?tabs=csharp)

## How to run app
1. Rename `local.settings.sample.json` to `local.settings.json` and replace value with your Azure AD applications and Azure Data Explorer settings.
2. Press [F5] key to start Azure Functions app.
3. Send HTTP GET (http://localhost:7071/api/GetTestData?durationDay=1) request to the Azure Functions. You can set appropriate `durationDay` variable as positive value. It will change duration of data (ex. 2 means that an app get data from 2 days ago to now)