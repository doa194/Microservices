using CQRS_Manual.CQRS.Commands.Response;
using MediatR;

namespace CQRS_Manual.CQRS.Commands.Requests
{
    public class CreateCommandRequest : IRequest<CreateCommandResponse>
    
    {
        public string ProductName { get; set; } = string.Empty;
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
}
