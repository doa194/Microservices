using Coordinator.Model;
using Coordinator.Model.Context;
using Microsoft.EntityFrameworkCore;
using Coordinator.Services;

namespace Coordinator.Transaction
{
    public class Transaction(IHttpClientFactory _httpClientFactory, Context _context) : ITransaction
    {
        HttpClient orderClient = _httpClientFactory.CreateClient("Order-Service");
        HttpClient paymentClient = _httpClientFactory.CreateClient("Payment-Service");
        HttpClient stockClient = _httpClientFactory.CreateClient("Stock-Service");

        public async Task<Guid> CreateTransactionAsync()
        {
            // Generate a new transaction ID
            Guid transactionId = Guid.NewGuid();

            var serviceList = await _context.Services.ToListAsync();
            serviceList.ForEach(service => service.ServiceStatus = new List<ServiceStatus>()
            {
                new(transactionId)
                {
                    IsServiceOK = Enum.ServiceEnum.Waiting,
                    ServiceTransactionState = Enum.TransactionEnum.Waiting
                }
            });

            await _context.SaveChangesAsync();
            return transactionId;
        }
        public async Task PrepareServiceAsync(Guid transactionId)
        {
            var services = await _context.ServiceStatus.Include(state => state.Services).Where(state => state.TransactionId == transactionId).ToListAsync();

            foreach (var service in services)
            {
                try
                {
                    var response = await (service.Services.ServiceName switch
                    {
                        "Order-Service" => orderClient.GetAsync("/ready"),
                        "Payment-Service" => paymentClient.GetAsync("/ready"),
                        "Stock-Service" => stockClient.GetAsync("/ready")
                    });

                    var result = bool.Parse(await response.Content.ReadAsStringAsync());
                    service.IsServiceOK = result ? Enum.ServiceEnum.Ready : Enum.ServiceEnum.NotReady;
                }
                catch (Exception)
                {
                    service.IsServiceOK = Enum.ServiceEnum.NotReady;
                }
            }
        }

        public async Task<bool> CheckServiceAsync(Guid transactionId)
        {
            return (await _context.ServiceStatus.Where(state => state.TransactionId == transactionId).ToListAsync()).TrueForAll(service => service.IsServiceOK == Enum.ServiceEnum.Ready);
        }

        public async Task CommitTransactionAsync(Guid transactionId)
        {
            var transactions = await _context.ServiceStatus.Where(state => state.TransactionId == transactionId).Include(state => state.Services).ToListAsync();

            foreach (var transaction in transactions)
            {
                try
                {
                    var response = await (transaction.Services.ServiceName switch
                    {
                        "Order-Service" => orderClient.PostAsync($"/commit/{transaction.TransactionId}", null),
                        "Payment-Service" => paymentClient.PostAsync($"/commit/{transaction.TransactionId}", null),
                        "Stock-Service" => stockClient.PostAsync($"/commit/{transaction.TransactionId}", null)
                    });

                    var result = bool.Parse(await response.Content.ReadAsStringAsync());   
                    transaction.ServiceTransactionState = result ? Enum.TransactionEnum.Success : Enum.TransactionEnum.Failed;
                }
                catch (Exception)
                {
                    transaction.ServiceTransactionState = Enum.TransactionEnum.Failed;
                }
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> TransactionStatusAsync(Guid transactionId)
        {
            return (await _context.ServiceStatus
                .Where(state => state.TransactionId == transactionId).ToListAsync()).TrueForAll(status => status.ServiceTransactionState == Enum.TransactionEnum.Success);
        }

        public async Task TransactionRollbackAsync(Guid transactionId)
        {
           var transactions = await _context.ServiceStatus
                .Where(state => state.TransactionId == transactionId)
                .Include(state => state.Services)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                try
                {
                    if (transaction.ServiceTransactionState == Enum.TransactionEnum.Success)
                        _ = await (transaction.Services.ServiceName switch
                        {
                            "Order-Service" => orderClient.PostAsync($"/rollback/{transaction.TransactionId}", null),
                            "Payment-Service" => paymentClient.PostAsync($"/rollback/{transaction.TransactionId}", null),
                            "Stock-Service" => stockClient.PostAsync($"/rollback/{transaction.TransactionId}", null)
                        });
                    transaction.ServiceTransactionState = Enum.TransactionEnum.Failed;
                }
                catch (Exception)
                {
                    transaction.ServiceTransactionState = Enum.TransactionEnum.Failed;
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
