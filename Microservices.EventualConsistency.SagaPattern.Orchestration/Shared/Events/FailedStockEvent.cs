﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class FailedStockEvent : CorrelatedBy<Guid>
    {
        public FailedStockEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get;  }
        public string Message { get; set; }
    }
}
