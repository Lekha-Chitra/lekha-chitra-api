using LekhaChitra.Application.Features.Auth.ForgotPassword.ResetPassword;
using LekhaChitra.Application.Features.Auth.ForgotPassword.SendOtp.Command;
using LekhaChitra.Application.Features.Auth.ForgotPassword.VerifyOtp.Command;
using LekhaChitra.Application.Features.Auth.Login.Command;
using LekhaChitra.Application.Features.Auth.Register.Command;
using LekhaChitra.Application.Interfaces.SmtpEmailService;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LekhaChitra.API.Controllers.Auth
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IEmailService _emailService;

        public AuthController(IMediator mediator, IEmailService emailService)
        {
            _mediator = mediator;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("forgotPassword/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);

        }


     
        [HttpPost]
        [Route("forgotPassword/sendOtp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);

        }
        [HttpPost]
        [Route("forgotPassword/verifyOtp")]
        public async Task<IActionResult> Verify([FromBody] VerifyOtpCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("model not valid");
            }
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);

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

            if (result.Data is not null)
            {
                Response.Cookies.Append("MyAuthValue", result.Data, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // dev only
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1),
                    Path = "/"
                });
            }
            return StatusCode(result.StatusCode, result);

        }
    }
}
