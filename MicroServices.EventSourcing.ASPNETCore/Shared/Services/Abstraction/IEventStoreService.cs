using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Abstraction
{
    // This interface defines the contract for an event store service that allows appending events to a stream, creating new event data, and subscribing to streams for event processing.
    
    public interface IEventStoreService
    {
        // This method appends a collection of EventData objects to a specified stream in the event store. It takes the stream name and an enumerable collection of EventData objects as parameters.
        Task AppendToStreamAsync(string streamName, IEnumerable<EventData> events);

        // This method creates a new EventData object from the provided event object. It serializes the event object to JSON and assigns a new UUID to the event. The EventData object is used to represent an event in the event store, which can be appended to a stream.
        EventData NewEventData(object _event);

        // This method is used to subscribe to a stream and process events as they are appended to the stream. The method takes a stream name, a function that processes the event, and a cancellation token to cancel the subscription if needed.
        Task SubscribeToStreamAsync(string streamName, Func<StreamSubscription, ResolvedEvent, CancellationToken, Task> _subscribedEvent); 
    }
}
