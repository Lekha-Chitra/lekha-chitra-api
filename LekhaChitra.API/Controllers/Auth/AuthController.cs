using LekhaChitra.Application.Features.Auth.Login.Command;
using LekhaChitra.Application.Features.Auth.Register.Command;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LekhaChitra.API.Controllers.Auth
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }           
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
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
