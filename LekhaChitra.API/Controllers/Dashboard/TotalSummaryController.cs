using LekhaChitra.Application.Features.Clients.Query.GetClients.GetAllClients;
using LekhaChitra.Application.Features.Dashboard.Query.GetSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LekhaChitra.API.Controllers.Dashboard
{
    [Route("api/V1/Dashboard/[controller]")]
    [ApiController]
    public class TotalSummaryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TotalSummaryController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        [Route("getSummary")]
        [Authorize]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime? fromDate,
                                                            [FromQuery] DateTime? toDate,
                                                            CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(new GetDashboardSummaryQuery(fromDate, toDate), ct);
            return StatusCode(result.StatusCode, result);

        }
    }
}
