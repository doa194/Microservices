namespace CQRS_Manual.CQRS.Commands.Response
{
    public class CreateCommandResponse
    {
        public bool IsSuccess { get; set; }
        public Guid ProductId { get; set; }
    }
}
