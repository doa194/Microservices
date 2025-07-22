namespace CQRS_Manual.CQRS.Queries.Response
{
    public class IDQueryResponse
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
