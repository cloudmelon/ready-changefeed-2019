using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace slotmachines { 

    class Program
    {
        private static readonly Uri _endpointUri = new Uri("https://tisande-changefeed.documents.azure.com:443/");
        private static readonly string _primaryKey = "yiEVaKKdYBVArpMeadom7p6IXPxZYdV4g9epMXRBXivp3kDs6XMQVCPPGvqFLedouhlxL50QzrrCLNdbA9qEGQ==";
        public static async Task Main(string[] args)
        {
            using (DocumentClient client = new DocumentClient(_endpointUri, _primaryKey))
            {
                for (int i = 0; i < 2000000; i++)
                {
                    await client.OpenAsync();
                    Uri gamesCollection = UriFactory.CreateDocumentCollectionUri("slotmachines", "games");

                    var Games = new Bogus.Faker<Game>()
                  .RuleFor(u => u.playerId, f => f.Random.Number(111111, 999999).ToString())
                  .RuleFor(u => u.machineId, f => f.Random.Number(1001, 9999).ToString())
                  .RuleFor(u => u.gameCost, f => f.Random.Number(1, 10))
                  .RuleFor(u => u.prizeAward, f => f.Random.Number(0, 500)*f.Random.Number(0,1)*f.Random.Number(0,1)*f.Random.Number(0,1)*f.Random.Number(0,1))
                  .Generate(100);

                    foreach (var game in Games)
                    {
                        ResourceResponse<Document> result = await client.CreateDocumentAsync(gamesCollection, game);
                        Console.Write("*");
                    }
                }
            }
        }
    }
}
