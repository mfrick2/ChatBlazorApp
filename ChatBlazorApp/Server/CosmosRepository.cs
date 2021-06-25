using Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBlazorApp.Server
{
    //This connects the data to a Cosmos Database
    public class CosmosRepository
    {
        CosmosContainer chatContainer;
        public CosmosRepository()
        {
            CosmosClient client = new CosmosClient("https://blazorchat.documents.azure.com:443/",
                "dUKDuscVEsTq8TLLMqW2UkmlVHTQ04oScmS1xC25qI3o6RL6r0LlIgJfxvEL8uz8JnmZ2x5YhcE8ehpYhJJH7Q==");
            CosmosDatabase cosmosDatabase = client.GetDatabase("ChatApp");
            chatContainer = cosmosDatabase.GetContainer("Chat");
        }

        public async Task<IEnumerable<T>> GetAll<T>(string room)
        {
            List<T> items = new List<T>();
            QueryDefinition query = new QueryDefinition($"select * from c where c.roomName = '{room}'");
            var response = chatContainer.GetItemQueryIterator<T>(query);
            await foreach (var item in response)
            {
                items.Add(item);
            }
            return items;
        }

        public async Task<bool> Upsert<T>(T item)
        {
            var response = await chatContainer.UpsertItemAsync(item);
            return (response.GetRawResponse().Status == 200);
        }
    }
}
