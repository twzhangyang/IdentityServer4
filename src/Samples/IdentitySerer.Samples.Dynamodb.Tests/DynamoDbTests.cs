using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Xunit;

namespace IdentitySerer.Samples.Dynamodb.Tests
{
    public class DynamoDbTests
    {
        private readonly AmazonDynamoDBClient _amazonDynamoDb;

        private string _tableName = "table1";

        public DynamoDbTests()
        {
            var clientConfig = new AmazonDynamoDBConfig {ServiceURL = "http://localhost:8000"};
            _amazonDynamoDb = new AmazonDynamoDBClient(clientConfig);
        }

        [Fact]
        public async Task Initialise()
        {
            var request = new ListTablesRequest
            {
                Limit = 10
            };

            var response = await _amazonDynamoDb.ListTablesAsync(request);

            var results = response.TableNames;

            if (!results.Contains(_tableName))
            {
                var createRequest = new CreateTableRequest
                {
                    TableName = _tableName,
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = "N"
                        }
                    },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = "HASH" //Partition key
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 2,
                        WriteCapacityUnits = 2
                    }
                };

                await _amazonDynamoDb.CreateTableAsync(createRequest);
            }
        }


        [Theory]
        [InlineData(1)]
        public async Task CreateItem(int id)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { N = id.ToString() }},
                    { "Title", new AttributeValue { S = "Title" }}
                }
            };

            await _amazonDynamoDb.PutItemAsync(request);
        }
        
        [Fact]
        public async Task GetItem()
        {
//            var table = Table.LoadTable(_amazonDynamoDb, _tableName);
//            var item = await table.GetItemAsync(new Dictionary<string, DynamoDBEntry>()
//            {
//                {"Id", new Primitive("1")}
//            });

            var queryRequest = new ScanRequest
            {
                TableName = _tableName,
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                    {
                        ":v_Id", new AttributeValue { N = 1.ToString() }}
                },
                FilterExpression = "Id = :v_Id",
            };

            var result = await _amazonDynamoDb.ScanAsync(queryRequest);
        }
        
        [Theory]
        [InlineData(1)]
        public async Task Delete(int id)
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue> { { "Id", new AttributeValue { N = id.ToString() } } }
            };

            var response = await _amazonDynamoDb.DeleteItemAsync(request);
        }
    }
}