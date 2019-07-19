using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.ChangeFeedProcessor.FeedProcessing;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace slotmachines
{
    class ChangeFeedObserver : IChangeFeedObserver
    {

        public Task CloseAsync(IChangeFeedObserverContext context, ChangeFeedObserverCloseReason reason)
        {
            return Task.CompletedTask;  // Note: requires targeting .Net 4.6+.
        }

        public Task OpenAsync(IChangeFeedObserverContext context)
        {
            return Task.CompletedTask;
        }

        public Task ProcessChangesAsync(IChangeFeedObserverContext context, IReadOnlyList<Document> newGamesList, CancellationToken cancellationToken)
        {
            Console.WriteLine("Processing " + newGamesList.Count + " score updates");
            UpdateMaterializedView updateMaterializedView = new UpdateMaterializedView();
            updateMaterializedView.Update(newGamesList);
            
            return Task.CompletedTask;
        }
    }
}
