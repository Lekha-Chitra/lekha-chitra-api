using LekhaChitra.Application.Features.Clients.AddClients;
using LekhaChitra.Application.Features.Clients.GetClients.GetAllClients;
using LekhaChitra.Application.Features.SubCategories.AddSubCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LekhaChitra.API.Controllers.Clients
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [Route("addClient")]
        [Authorize]
        public async Task<IActionResult> AddClient([FromBody] AddClientCommand  command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);

        }

        [HttpGet]
        [Route("getAllClient")]
        [Authorize]
        public async Task<IActionResult> GetClient([FromQuery] int pageNumber = 1,
                                                    [FromQuery] int pageSize = 10,CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(new GetAllClientQuery(pageNumber,pageSize),ct);
            return StatusCode(result.StatusCode, result);

        }
    }
}
