using CQRS_Manual.CQRS.Queries.Response;
using MediatR;

namespace CQRS_Manual.CQRS.Queries.Requests
{
    public class IDQueryRequest : IRequest<IDQueryResponse>
    {
        public Guid ProductId { get; set; }

    }
}
