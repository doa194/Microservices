namespace Coordinator.Model
{
    public class Services
    {
        public Guid ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public ICollection<ServiceStatus>? ServiceStatus { get; set; }
    }
}
