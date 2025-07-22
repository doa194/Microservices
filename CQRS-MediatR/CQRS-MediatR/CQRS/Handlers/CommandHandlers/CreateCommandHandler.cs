using CQRS_Manual.CQRS.Commands.Requests;
using CQRS_Manual.CQRS.Commands.Response;
using CQRS_Manual.Models;
using MediatR;

namespace CQRS_Manual.CQRS.Handlers.CommandHandlers
{
    public class CreateCommandHandler : IRequestHandler<CreateCommandRequest, CreateCommandResponse>
    {

        public async Task<CreateCommandResponse> Handle(CreateCommandRequest createRequest, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            Context.Products.Add(new()
            {
                ProductId = id,
                ProductName = createRequest.ProductName,
                ProductQuantity = createRequest.ProductQuantity,
                ProductPrice = createRequest.ProductPrice,
                CreatedDate = DateTime.UtcNow
            });

            return new CreateCommandResponse
            {
                ProductId = id,
                IsSuccess = true
            };
        }
    }
}
