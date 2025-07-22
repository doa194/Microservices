using CQRS_Manual.CQRS.Queries.Requests;
using CQRS_Manual.CQRS.Queries.Response;
using CQRS_Manual.Models;
using MediatR;

namespace CQRS_Manual.CQRS.Handlers.QueryHandlers
{
    public class IDQueryHandler : IRequestHandler<IDQueryRequest, IDQueryResponse>
    {
        public async Task<IDQueryResponse> Handle(IDQueryRequest iDQueryRequest, CancellationToken cancellationToken)
        {
            var product = Context.Products.FirstOrDefault(x => x.ProductId == iDQueryRequest.ProductId);

            return new IDQueryResponse
            {
                ProductId = product?.ProductId ?? Guid.Empty,
                ProductName = product?.ProductName ?? string.Empty,
                ProductQuantity = product?.ProductQuantity ?? 0,
                ProductPrice = product?.ProductPrice ?? 0.0m,
                CreatedDate = product?.CreatedDate ?? DateTime.UtcNow
            };
        }
    }
}
