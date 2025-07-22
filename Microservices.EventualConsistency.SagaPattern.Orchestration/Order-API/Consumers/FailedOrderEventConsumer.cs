using MassTransit;
using Shared.Events;

namespace Order_API.Consumers
{
    public class FailedOrderEventConsumer(Order_API.Context.Context _context) : IConsumer<FailedOrderEvent>
    {
        public async Task Consume(ConsumeContext<FailedOrderEvent> context)
        {
            Order_API.Models.Order order = await _context.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = Enums.OrderStatusEnum.Failed;
                await _context.SaveChangesAsync();
            }
            else
            {

            }
        }
    }
}
