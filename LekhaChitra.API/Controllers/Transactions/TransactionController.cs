using LekhaChitra.Application.Features.Clients.AddClients;
using LekhaChitra.Application.Features.Transaction.AddTransaction;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LekhaChitra.API.Controllers.Transactions
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("addTransaction")]
        [Authorize]
        public async Task<IActionResult> AddTransaction([FromBody] AddTransactionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);

        }
    }
}
