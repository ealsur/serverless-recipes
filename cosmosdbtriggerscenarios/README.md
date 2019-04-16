# Serverless recipes with the Azure Cosmos DB Trigger
[<- Back to the root](../README.md)

The following project contains samples using Azure Function's [Cosmos DB Trigger](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb#trigger).

It contains different samples:

1. [MultipleTriggers](./src/MultipleTriggers.cs): Have multiple Triggers triggering in parallel and independently.

## How to run this sample

You can use Visual Studio Code, Visual Studio or even the [Azure Functions' CLI](https://github.com/Azure/azure-functions-core-tools). You only need to customize the [local.settings.json](./src/local.settings.json) file to match the Cosmos DB *Connection String*, *Database name*, and *Collection name* on which you want the queries to run.

## Useful links

* [Azure Cosmos DB + Functions Cookbook — multi trigger](https://medium.com/@Ealsur/azure-cosmos-db-functions-cookbook-multi-trigger-f8938673de57)
* [More Cosmos DB Trigger samples](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb#trigger---example)