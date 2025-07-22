using MassTransit.Testing;
using Order_API.Enums;

namespace Order_API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }
        public ICollection<OrderDetails>? OrderDetails { get; set; }
    }
}
