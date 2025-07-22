using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public static class RabbitMQConfig
    {
        public const string StateMachineQueue = $"state-machine-queue";
        public const string StockToOrderCreateEventQueue = $"stock-to-order-create-event-queue";
        public const string OrderToOrderCompletedEventQueue = $"order-to-order-completed-event-queue";  
        public const string OrderToOrderFailedEventQueue = $"order-to-order-failed-event-queue";
        public const string PaymentToPaymentStartedEventQueue = $"payment-to-payment-started-event-queue";
    }
}
