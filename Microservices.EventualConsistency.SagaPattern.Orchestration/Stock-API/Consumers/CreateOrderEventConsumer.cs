using MassTransit;
using Shared.Events;
using Stock_API.Context;
using Stock_API.Models;
using MongoDB.Driver;
using Shared.Configurations;

namespace Stock_API.Consumers
{
    public class CreateOrderEventConsumer(MongoDBContext mongoDBContext, ISendEndpointProvider sendEndpointProvider) : IConsumer<CreateOrderEvent>
    {
        public async Task Consume(ConsumeContext<CreateOrderEvent> context)
        {
            List<bool> results = new List<bool>();
            var stock = mongoDBContext.GetCollection<Stock>();

            foreach (var item in context.Message.orderDetails)
                results.Add(await (await stock.FindAsync(x => x.ProductId == item.ProductId && x.Count >= item.Count)).AnyAsync());
            
            var sendEndpoint = sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQConfig.StateMachineQueue}"));
            
            
            if (results.TrueForAll(s => s.Equals(true)))
            {
                foreach(var item in context.Message.orderDetails)
                {
                    var _stock = await (await stock.FindAsync(x => x.ProductId == item.ProductId)).FirstOrDefaultAsync();
                    _stock.Count = item.Count;

                    await stock.FindOneAndReplaceAsync(x => x.ProductId == item.ProductId, _stock);
                }

                ReservedStockEvent reservedStockEvent = new(context.Message.CorrelationId)
                {
                    orderDetails = context.Message.orderDetails
                };
                await sendEndpointProvider.Send(reservedStockEvent);
            }
            else
            {
                FailedStockEvent failedStockEvent = new(context.Message.CorrelationId)
                {
                    Message = "Failed to reserve stock"
                }
            ;

            }
        }
    }
}
