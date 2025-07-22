using MassTransit;
using Stock_API.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class ReservedStockEvent : CorrelatedBy<Guid>
    {
        public ReservedStockEvent(Guid correlationId)
        {
            CorrelationId = correlationId;

        }
        public Guid CorrelationId {get;}
        public List<OrderDetailsMessage> orderDetails { get; set; } = new List<OrderDetailsMessage>();
    }
}
