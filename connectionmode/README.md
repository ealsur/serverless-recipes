# Serverless recipes with the Azure Cosmos DB and custom Host configuration
[<- Back to the root](../README.md)

The following project contains samples using Azure Functions with a custom [host.json](./src/host.json).

It contains 1 sample:

1. [CustomConnectionMode](./src/CustomConnectionMode.cs): Use an HTTP GET to query for a particular document in a container by matching part of the GET's route to the Id of the document, using the `DocumentClient` in **Direct/TCP** mode.

## How to run this sample

You can use Visual Studio Code, Visual Studio or even the [Azure Functions' CLI](https://github.com/Azure/azure-functions-core-tools). You only need to customize the [local.settings.json](./src/local.settings.json) file to match the Cosmos DB *Connection String*, *Database name*, and *Collection name* on which you want the queries to run.

## Useful links

* [Azure Cosmos DB + Functions Cookbook — HTTP querying](https://medium.com/@Ealsur/azure-cosmos-db-functions-cookbook-http-querying-4afc5ed445d7)