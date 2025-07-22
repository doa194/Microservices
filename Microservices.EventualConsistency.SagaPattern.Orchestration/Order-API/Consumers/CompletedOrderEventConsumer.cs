using MassTransit;
using Shared.Events;

namespace Order_API.Consumers
{
    public class CompletedOrderEventConsumer(Order_API.Context.Context _context) : IConsumer<CompletedOrderEvent>
    {
        public async Task Consume(ConsumeContext<CompletedOrderEvent> context)
        {
            Order_API.Models.Order order = await _context.Orders.FindAsync(context.Message.OrderId);
            if(order != null)
            {
                order.OrderStatus = Enums.OrderStatusEnum.Successful;
                await _context.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the order is not found
            }
        }
    }
}
