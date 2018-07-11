# Serverless recipes with the Azure Cosmos DB Output Binding
[<- Back to the root](../README.md)

The following project contains samples using Azure Function's [Cosmos DB Output Binding](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb#output).

It contains 2 different samples:

1. [HttpTriggerToAsyncCollector](./src/HttpTriggerToAsyncCollector.cs): Use an HTTP POST to send a batch of documents to be saved to a container in a single operation.
2. [HttpTriggerWithSingleDocument](./src/HttpTriggerWithSingleDocument.cs): Use an HTTP POST to send a single document to be saved to a container.

## How to run this sample

You can use Visual Studio Code, Visual Studio or even the [Azure Functions' CLI](https://github.com/Azure/azure-functions-core-tools). You only need to customize the [local.settings.json](./src/local.settings.json) file to match the Cosmos DB *Connection String*, *Database name*, and *Collection/Container name* on which you want the documents to be saved to.

## Useful links

* [Azure Cosmos DB + Functions Cookbook — output collector](https://medium.com/@Ealsur/azure-cosmos-db-functions-cookbook-output-collector-fbcdd663280d)
* [More Cosmos DB Output Binding samples](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb#output---examples)