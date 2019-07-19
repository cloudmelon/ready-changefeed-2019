using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.ChangeFeedProcessor;

namespace slotmachines

{
    class SlotMachinesChangeFeedProcessor
    {
        static void Main(string[] args)
        {
            var p = new SlotMachinesChangeFeedProcessor();
            p.Run();
        }
        public void Run()
        {
            RunAsync().Wait();
        }

        static async Task RunAsync()
        {
            DocumentCollectionInfo feedCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "slotmachines",
                CollectionName = "games",
                Uri = new Uri("<your-connection-string>"),
                MasterKey = "<your-primary-key>"
            };

            DocumentCollectionInfo leaseCollectionInfo = new DocumentCollectionInfo()
            {
                DatabaseName = "slotmachines",
                CollectionName = "lease",
                Uri = new Uri("<your-connection-string>"),
                MasterKey = "<your-primary-key>"
            };

            ChangeFeedProcessorOptions feedProcessorOptions = new ChangeFeedProcessorOptions();

            feedProcessorOptions.LeaseRenewInterval = TimeSpan.FromSeconds(5);
            feedProcessorOptions.StartFromBeginning = false;

            var builder = new ChangeFeedProcessorBuilder();
            var processor = await builder
                .WithHostName("materialized-view")
                .WithFeedCollection(feedCollectionInfo)
                .WithLeaseCollection(leaseCollectionInfo)
                .WithObserver<ChangeFeedObserver>()
                .WithProcessorOptions(feedProcessorOptions)
                .BuildAsync();

            await processor.StartAsync();

            Console.WriteLine("Change Feed Processor started. Press <Enter> key to stop...");

            Console.ReadLine();

            await processor.StopAsync();
        }
    }
}