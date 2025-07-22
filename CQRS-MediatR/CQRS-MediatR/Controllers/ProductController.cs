using CQRS_Manual.CQRS.Commands.Requests;
using CQRS_Manual.CQRS.Queries.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_MediatR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllProducts([FromQuery] AllQueryRequest request)
        {
            return Ok(mediator.Send(request));
        }
        [HttpGet("{ProductId}")]
        public IActionResult GetProductById([FromRoute] IDQueryRequest request)
        {
            return Ok(mediator.Send(request));
        }
        [HttpPost]
        public IActionResult CreateProduct([FromRoute] CreateCommandRequest request)
        {
            return Ok(mediator.Send(request));
        }
        [HttpDelete("{ProductId}")]
        public IActionResult DeleteProduct([FromRoute] DeleteRCommandequest request)
        {
            return Ok(mediator.Send(request));
        }
    }
}
