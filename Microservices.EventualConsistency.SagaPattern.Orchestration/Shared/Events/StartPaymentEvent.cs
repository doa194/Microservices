using MassTransit;
using Stock_API.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class StartPaymentEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public StartPaymentEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public List<OrderDetailsMessage> orderDetails { get; set; } = new List<OrderDetailsMessage>();
    }
}
