using Stock_API.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class FailedPaymentEvent
    {
        public Guid CorrelationId { get; }
        public FailedPaymentEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public string Message { get; set; } = string.Empty;
        public List<OrderDetailsMessage> orderDetails { get; set; } = new List<OrderDetailsMessage>();
    }
}
