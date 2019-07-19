using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace slotmachines
{
    public class UpdateMaterializedView
    {
        private DocumentClient client;
        private string databaseName = "slotmachines";
        private string playerCollectionName = "players";
        private string slotMachineCollectionName = "machines";
        private static readonly Uri endpointUri = new Uri("<your-account-name>");
        private static readonly string primaryKey = "<your-primary-key>";

        public UpdateMaterializedView()
        {
            this.client = new DocumentClient(endpointUri, primaryKey);
        }

        public async Task Update(IReadOnlyList<Document> newGamesList)
    {
            foreach (var newGame in newGamesList)
            {          
                var newGameEvent = JsonConvert.DeserializeObject<Game>(newGame.ToString());
                
               Uri databaseLink = UriFactory.CreateDatabaseUri(databaseName);
               Uri playerCollectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, playerCollectionName);
               Uri slotMachineCollectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, slotMachineCollectionName);
               
                // Update player current balance

                Uri playerDocumentUri = UriFactory.CreateDocumentUri(databaseName, playerCollectionName, newGameEvent.playerId);

                try
                {
                    Document playerDoc = await client.ReadDocumentAsync(playerDocumentUri, new RequestOptions { PartitionKey = new PartitionKey(newGameEvent.playerId) });
                    var existingPlayer = JsonConvert.DeserializeObject<Player>(playerDoc.ToString());
                    Player updatedPlayer = new Player(newGameEvent.playerId, newGameEvent.prizeAward - newGameEvent.gameCost + existingPlayer.balance);
                    await client.UpsertDocumentAsync(playerCollectionLink, updatedPlayer);
                }

                catch (DocumentClientException ex)
                {

                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        
                        Player newPlayer = new Player(newGameEvent.playerId, newGameEvent.prizeAward - newGameEvent.gameCost);
                        await client.UpsertDocumentAsync(playerCollectionLink, newPlayer);
                    }
                }

                // Update Slot Machine summmary

                Uri slotMachineDocumentUri = UriFactory.CreateDocumentUri(databaseName, slotMachineCollectionName, newGameEvent.machineId);

                try
                {
                    Document slotMachineDoc = await client.ReadDocumentAsync(slotMachineDocumentUri, new RequestOptions { PartitionKey = new PartitionKey(newGameEvent.machineId) });
                    var existingSlotMachine = JsonConvert.DeserializeObject<SlotMachine>(slotMachineDoc.ToString());
                    SlotMachine updatedSlotMachine = new SlotMachine(newGameEvent.machineId, existingSlotMachine.totalGames + 1, existingSlotMachine.totalMoneyInput + newGameEvent.gameCost, existingSlotMachine.totalPrizes + newGameEvent.prizeAward, existingSlotMachine.totalProfit + newGameEvent.gameCost - newGameEvent.prizeAward );
                    await client.UpsertDocumentAsync(slotMachineCollectionLink, updatedSlotMachine);
                }

                catch (DocumentClientException ex)
                {
                    if (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        SlotMachine newSlotMachine = new SlotMachine(newGameEvent.machineId, 1,  newGameEvent.gameCost, newGameEvent.prizeAward, newGameEvent.gameCost - newGameEvent.prizeAward);
                        await client.UpsertDocumentAsync(slotMachineCollectionLink, newSlotMachine);
                    }
                }


            }
        }

    }
}
