# Serverless recipes with a Cosmos DB custom client
[<- Back to the root](../README.md)

The following project contains a sample on maintaining and reusing a Cosmos DB client. The concept is valid for any HTTP-based client library/SDK.

It contains a single sample:

1. [HttpTriggerToStaticClient](./src/HttpTriggerToStaticClient.cs): Create and maintain a custom `DocumentClient` instance to be reused in all the Function's executions. The Function will receive a POST similar to the [HttpTriggerToAsyncCollector](../cosmosdboutputbindings/src/HttpTriggerToAsyncCollector.cs) sample and store a group of documents into a container.

## How to run this sample

You can use Visual Studio Code, Visual Studio or even the [Azure Functions' CLI](https://github.com/Azure/azure-functions-core-tools). You only need to customize the [local.settings.json](./src/local.settings.json) file to match the Cosmos DB *Account Endpoint*, *Account Key*, *Database name*, and *Collection/Container name* on which you want the client to be created and documents to be saved.

## Useful links

* [Azure Cosmos DB + Functions Cookbook — static client](https://medium.com/@Ealsur/azure-cosmos-db-functions-cookbook-static-client-874072aef28e)
* [Managing connections in Azure Functions](https://github.com/Azure/azure-functions-host/wiki/Managing-Connections)