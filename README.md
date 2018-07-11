# Serverless recipes with Azure Cosmos DB and Azure Functions

This repository acts as compendium of different recipes using [Azure Cosmos DB](https://docs.microsoft.com/azure/cosmos-db/) and [Azure Functions](https://docs.microsoft.com/azure/azure-functions/).

The content is distributed in different folders, each for a different scenario:

* [Cosmos DB Input Binding](./cosmosdbinputbinding): Contains samples on how to use the Cosmos DB Input Binding to 
run queries and read data from your Cosmos DB containers.
* [Cosmos DB Output Binding](./cosmosdboutputbinding): Contains samples on how to use the Cosmos DB Output Binding to save a single document or multiple documents to your Cosmos DB containers.
* [Client management](./cosmosdbstaticclient): Contains samples on how to best use a custom Cosmos DB SDK client within Azure Functions optimizing for best performance and avoiding common pitfalls.
* [Cosmos DB Trigger](./cosmosdbtrigger): Contains a complete sample using Cosmos DB, [Azure Cognitive Services](https://azure.microsoft.com/services/cognitive-services/), and [Azure Signal R](https://azure.microsoft.com/services/signalr-service/) to create a flow of data analysis based on events happening in a Cosmos DB container.