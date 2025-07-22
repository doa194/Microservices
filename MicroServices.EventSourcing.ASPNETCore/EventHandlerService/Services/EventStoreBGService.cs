using Shared.Services.Abstraction;
using System.Text.Json;
using System.Reflection;
using Shared.Events;
using MongoDB.Driver;

namespace EventHandlerService.Services
{
    // This service listens to events from the Event Store and updates the MongoDB database accordingly. It is designed to run in the background and process events asynchronously. The service subscribes to a specific stream ("product-stream") and handles events related to products.

    public class EventStoreBGService(IEventStoreService _eventStoreService, IMongoDBService mongoDBService) : BackgroundService
    {
        // The constructor takes an instance of IEventStoreService and IMongoDBService to interact with the event store and MongoDB database, respectively. The ExecuteAsync method is overridden to define the background service's main logic, which subscribes to the event stream and processes incoming events.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // This method is called when the background service starts. It subscribes to the "product-stream" in the event store and processes events as they arrive. The event data is deserialized into the appropriate type based on the event type, and then the MongoDB database is updated accordingly. The service listens for events indefinitely until it is stopped.
            
            // The event handling logic is encapsulated in a switch statement that checks the type of the event data and performs the necessary database operations. In this case, it handles the NewProductEvent by checking if the product already exists in the MongoDB collection and inserting it if it does not.
            
            // The service is designed to be resilient and can handle multiple events in a single run, processing each event asynchronously.

            await _eventStoreService.SubscribeToStreamAsync("product-stream", async (streamSubscription, resolvedEvent, cancellationToken) =>
            {
                string eventType = resolvedEvent.Event.EventType;
                object eventData = JsonSerializer.Deserialize(resolvedEvent.Event.Data.ToArray(), Assembly.Load("Shared").GetTypes().FirstOrDefault(x => x.Name == eventType));

                var productCollection = mongoDBService.GetCollection<Shared.Models.Product>("products");

                switch (eventData)
                {
                    case NewProductEvent np:
                        var product = await (await productCollection.FindAsync(p => p.ProductId == np.ProductId.ToString())).AnyAsync();
                        if (!product)
                        {
                            await productCollection.InsertOneAsync(new Shared.Models.Product
                            {
                                ProductId = np.ProductId.ToString(),
                                ProductName = np.ProductName,
                                IsAvailable = np.IsAvailable,
                                ProductPrice = np.Price
                            });
                        }
                        break;
                }
            });
        }
    }
}
