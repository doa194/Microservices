using MassTransit;
using SagaStateMachine.StateInstance;
using Shared.Configurations;
using Shared.Events;

namespace SagaStateMachine.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<StartOrderEvent> OrderStarted { get; set; }
        public Event<ReservedStockEvent> StockReserved { get; set; }
        public Event<FailedStockEvent> FailedStock { get; set; }
        public Event<SuccessfulPaymentEvent> PaymentSuccessful { get; set; }
        public Event<FailedPaymentEvent> PaymentFailed { get; set; }

        public State _OrderCreated { get; set; }
        public State _StockReserved { get; set; }
        public State _FailedStock { get; set; }
        public State _PaymentSuccessful { get; set; }
        public State _PaymentFailed { get; set; }

        public OrderStateMachine() 
        {
            // Define the states of the state machine
            InstanceState(x => x.CurrentState);

            Event(() => OrderStarted, x => x.CorrelateBy<int>(y => y.OrderId, @event => @event.Message.OrderId).SelectId(x => Guid.NewGuid()));
            Event(() => StockReserved, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => FailedStock, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => PaymentFailed, x => x.CorrelateById(y => y.Message.CorrelationId));
            Event(() => PaymentSuccessful, x => x.CorrelateById(y => y.Message.CorrelationId));


            // Define the states
            
            Initially(When(OrderStarted)
                .Then(context =>
                {
                    // Initialize the state instance with data from the event
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.CustomerId = context.Data.CustomerId.ToString();
                    context.Instance.TotalAmount = context.Data.TotalAmount;
                })
                .TransitionTo(_OrderCreated).Send(new Uri($"queue:{RabbitMQConfig.StockToOrderCreateEventQueue}"), context => new CreateOrderEvent(context.Instance.CorrelationId)
                {
                    CorrelationId = context.Instance.CorrelationId
                    //..
                }));
            During(_OrderCreated,
                When(StockReserved)
                    .TransitionTo(_StockReserved)
                    .Send(new Uri($"queue:{RabbitMQConfig.PaymentToPaymentStartedEventQueue}"), context => new StartPaymentEvent(context.Instance.CorrelationId)
                    {
                        CorrelationId = context.Instance.CorrelationId
                        //..
                    }),
                When(FailedStock)
                    .TransitionTo(_FailedStock)
                    .Send(new Uri($"queue:{RabbitMQConfig.OrderToOrderFailedEventQueue}"), context => new FailedOrderEvent
                    {
                        OrderId = context.Instance.OrderId
                        //..
                    }));
            During(_StockReserved,
                When(PaymentSuccessful)
                    .TransitionTo(_PaymentSuccessful)
                    .Send(new Uri($"queue:{RabbitMQConfig.OrderToOrderCompletedEventQueue}"), context => new CompletedOrderEvent
                    {
                        OrderId = context.Instance.OrderId,
                        //..
                    }).Finalize(),
                When(PaymentFailed)
                    .TransitionTo(_PaymentFailed)
                    .Send(new Uri($"queue:{RabbitMQConfig.OrderToOrderFailedEventQueue}"), context => new FailedOrderEvent
                    {
                        OrderId = context.Instance.OrderId
                        //..
                    }));
            SetCompletedWhenFinalized();
        }
    }
}
