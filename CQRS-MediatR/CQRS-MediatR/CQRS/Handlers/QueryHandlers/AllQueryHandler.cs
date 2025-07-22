using CQRS_Manual.CQRS.Queries.Requests;
using CQRS_Manual.CQRS.Queries.Response;
using CQRS_Manual.Models;
using MediatR;

namespace CQRS_Manual.CQRS.Handlers.QueryHandlers
{
    public class AllQueryHandler : IRequestHandler<AllQueryRequest, List<AllQueryResponse>>
    {

        public async Task<List<AllQueryResponse>> Handle(AllQueryRequest request, CancellationToken cancellationToken)
    {
        {
            return Context.Products.Select(p => new AllQueryResponse
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                CreatedDate = p.CreatedDate,
                ProductPrice = p.ProductPrice,
                ProductQuantity = p.ProductQuantity

            }).ToList();
        }
    }
    }
}
