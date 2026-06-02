using LekhaChitra.Application.Helpers.JwtHelper;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Auth.Login.Command
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ServiceResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }
        public async Task<ServiceResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
           var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                if (await _userManager.IsLockedOutAsync(user)) //will lock after 3 failed attempts
                {
                    return ServiceResponse.Locked("Your account is locked. Please try again later.");
                }
                var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);
               
                if (passwordValid)
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("JWTID", Guid.NewGuid().ToString()),
                    };

                    var token = await _jwtService.GenerateNewJsonWebToken(authClaims);
                    if (token == null)
                    {
                        return ServiceResponse.Unauthorized("Invalid email or password.");
                    }
                    await _userManager.ResetAccessFailedCountAsync(user);
                    return ServiceResponse<string>.Success(token, "User login successful");
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);
                    return ServiceResponse.Unauthorized("Invalid email or password.");
                }
            }
            else {
               
                return ServiceResponse.Unauthorized("Invalid email or password.");
            }

        }
    }
}
