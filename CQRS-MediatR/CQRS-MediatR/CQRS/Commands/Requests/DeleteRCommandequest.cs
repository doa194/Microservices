using CQRS_Manual.CQRS.Commands.Response;
using MediatR;

namespace CQRS_Manual.CQRS.Commands.Requests
{
    public class DeleteRCommandequest : IRequest<DeleteCommandResponse>
    {
        public Guid ProductId { get; set; }

    }
}
