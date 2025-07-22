using MassTransit;
using Shared.Configurations;
using Shared.Events;

namespace Stock_API.Consumers
{
    public class StartPaymentEventConsumer(ISendEndpointProvider sendEndpointProvider) : IConsumer<StartPaymentEvent>
    {
        public async Task Consume(ConsumeContext<StartPaymentEvent> context)
        {
            var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQConfig.StateMachineQueue}"));
            if (true)
            {
                SuccessfulPaymentEvent successfulPaymentEvent = new(context.Message.CorrelationId)
                {

                };
                await sendEndpoint.Send(successfulPaymentEvent);
            }
            else
            {
                FailedPaymentEvent failedPaymentEvent = new(context.Message.CorrelationId)
                {
                    Message = "Failed to start payment",
                    orderDetails = context.Message.orderDetails
                };
                await sendEndpoint.Send(failedPaymentEvent);
            }
        }
    }
}
