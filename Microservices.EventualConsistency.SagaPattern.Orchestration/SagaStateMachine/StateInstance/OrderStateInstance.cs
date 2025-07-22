using MassTransit;

namespace SagaStateMachine.StateInstance
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public int OrderId { get; set; }
        public string? CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
