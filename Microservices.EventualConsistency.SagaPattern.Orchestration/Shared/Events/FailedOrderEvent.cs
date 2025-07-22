using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events
{
    public class FailedOrderEvent
    {
        public int OrderId { get; set; } // This is the unique identifier for the order that failed
    }
}
