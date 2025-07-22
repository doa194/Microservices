using Coordinator.Enum;

namespace Coordinator.Model
{
    public class ServiceStatus
    {
        public ServiceStatus(Guid transactionId)
        {
            TransactionId = transactionId;
        }

        public Guid ServiceStatusId { get; set; }
        public Guid TransactionId { get; set; }

        public ServiceEnum IsServiceOK { get; set; }
        public TransactionEnum ServiceTransactionState { get; set; }

        public Services? Services { get; set; }

    }
}
