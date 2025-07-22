using EventStore.Client;
using Shared.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class EventStoreService : IEventStoreService
    {
        EventStoreClientSettings GetEventStoreClientSettings(string connectionString = "")
        {
            return EventStoreClientSettings.Create(connectionString);
        }

        EventStoreClient Client { get => new EventStoreClient(GetEventStoreClientSettings());}
        
        public async Task AppendToStreamAsync(string streamName, IEnumerable<EventData> events)
        {
            await Client.AppendToStreamAsync(
             streamName: streamName,
             eventData: events,
             expectedState: StreamState.Any);
        }

        public EventData NewEventData(object _event) =>
            new(
                eventId : Uuid.NewUuid(),
                type: _event.GetType().Name,
                data: JsonSerializer.SerializeToUtf8Bytes(_event)
                );

        async Task IEventStoreService.SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> _subscribedEvent)
        {
            await Client.SubscribeToStreamAsync(
                streamName: streamName,
                start: FromStream.Start,
                eventAppeared: _subscribedEvent,
                subscriptionDropped: (subscription, reason, exception) =>
                {
                    if (exception != null)
                    {
                        Console.WriteLine($"Subscription dropped due to exception: {exception.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"Subscription dropped due to reason: {reason}");
                    }
                }
            );
        }
    }
}
