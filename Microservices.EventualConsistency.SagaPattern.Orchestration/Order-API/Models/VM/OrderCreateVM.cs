namespace Order_API.Models.VM
{
    public class OrderCreateVM
    {
        public int CustomerId { get; set; }
        public ICollection<OrderDetailsVM> OrderDetailsVM { get; set; }
    }
}
