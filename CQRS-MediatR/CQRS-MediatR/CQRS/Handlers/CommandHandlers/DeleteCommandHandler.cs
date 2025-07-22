using CQRS_Manual.CQRS.Commands.Requests;
using CQRS_Manual.CQRS.Commands.Response;
using CQRS_Manual.Models;
using MediatR;

namespace CQRS_Manual.CQRS.Handlers.CommandHandlers
{
    public class DeleteCommandHandler : IRequestHandler<DeleteRCommandequest, DeleteCommandResponse>
    {
        public async Task<DeleteCommandResponse> Handle(DeleteRCommandequest deleteRequest, CancellationToken cancellationToken)
        {
            var product = Context.Products.FirstOrDefault(x => x.ProductId == deleteRequest.ProductId);

            if (product != null)
            {
                Context.Products.Remove(product);
            }

            return new DeleteCommandResponse { IsSuccess = true };
        }
    }
}
