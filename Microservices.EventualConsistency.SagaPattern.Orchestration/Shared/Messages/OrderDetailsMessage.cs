namespace Stock_API.Messages
{
    public class OrderDetailsMessage
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
