# Serverless recipes with the Azure Cosmos DB Input Binding
[<- Back to the root](../README.md)

The following project contains samples using Azure Function's [Cosmos DB Input Binding](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb#input).

It contains 3 different samples:

1. [HttpInputBindingByIdInBody](./src/HttpInputBindingByIdInBody.cs): Use an HTTP POST to query for a particular document in a container by matching part of the POST's body to the Id of the document.
2. [HttpInputBindingByIdInRoute](./src/HttpInputBindingByIdInRoute.cs): Use an HTTP GET to query for a particular document in a container by matching part of the GET's route to the Id of the document.
3. [HttpInputBindingWithQuery](./src/HttpInputBindingWithQuery.cs): Use an HTTP POST to query for a list of documents matching a custom query using the POST's body attributes as filters.

## How to run this sample

You can use Visual Studio Code, Visual Studio or even the [Azure Functions' CLI](https://github.com/Azure/azure-functions-core-tools). You only need to customize the [local.settings.json](./src/local.settings.json) file to match the Cosmos DB *Connection String*, *Database name*, and *Collection name* on which you want the queries to run.

## Useful links

* [Azure Cosmos DB + Functions Cookbook — HTTP querying](https://medium.com/@Ealsur/azure-cosmos-db-functions-cookbook-http-querying-4afc5ed445d7)
* [More Cosmos DB Input Binding samples](https://docs.microsoft.com/azure/azure-functions/functions-bindings-cosmosdb#input---examples)