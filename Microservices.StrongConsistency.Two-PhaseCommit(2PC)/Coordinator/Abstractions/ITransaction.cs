namespace Coordinator.Services
{
    public interface ITransaction
    {
        Task<Guid> CreateTransactionAsync();
        Task PrepareServiceAsync(Guid transactionId);
        Task<bool> CheckServiceAsync(Guid transactionId);
        Task CommitTransactionAsync(Guid transactionId);
        Task<bool> TransactionStatusAsync(Guid transactionId);
        Task TransactionRollbackAsync(Guid transactionId);
    }
}
