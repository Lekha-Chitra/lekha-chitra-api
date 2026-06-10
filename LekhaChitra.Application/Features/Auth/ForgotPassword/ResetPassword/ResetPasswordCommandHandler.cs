using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Auth.ForgotPassword.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ServiceResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler( UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ServiceResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return ServiceResponse.NotFound("User with the provided email does not exist.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token is not null)
            { 
                var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
                if (result.Succeeded)
                {
                    return ServiceResponse.Success("Password updated successfully.");
                }
                else { 
                    return ServiceResponse.BadRequest("Failed to reset password. Please try again.");
                }
            }
            return ServiceResponse.BadRequest("Failed to generate password reset token. Please try again.");
        }
    }
}
