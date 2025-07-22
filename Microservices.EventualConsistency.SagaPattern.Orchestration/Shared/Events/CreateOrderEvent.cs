using MassTransit;
using Stock_API.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Events
{
    public class CreateOrderEvent : CorrelatedBy<Guid> 
    {
        public CreateOrderEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; } // This is the unique identifier for the event, typically the OrderId
        public List<OrderDetailsMessage> orderDetails { get; set; } = new List<OrderDetailsMessage>(); // This is a list of order details, which includes information about the products in the order
    }
}
