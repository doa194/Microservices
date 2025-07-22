namespace Shared.Events
{
    public class NewProductEvent
    {
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
    }
}
