using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class SuccessfulPaymentEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public SuccessfulPaymentEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
