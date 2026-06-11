using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Auth.Register.Command
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ServiceResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ServiceResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
            {
                return ServiceResponse.Conflict("User with this email already exists.");
            }
            var phoneExists =  _userManager.Users.Any(u => u.PhoneNumber == request.PhoneNumber);
            if (phoneExists)
            {
                return ServiceResponse.Conflict("User with this phone number already exists.");
            }
            if (request.Password != request.ConfirmPassword)
            {
                return ServiceResponse.BadRequest("Password mismatch.");
            }

            var identityUser = new ApplicationUser
            {
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpperInvariant(),
                UserName = request.FullName,
                NormalizedUserName = request.FullName.ToUpperInvariant(),
                PhoneNumber = request.PhoneNumber,
                PhoneNumberConfirmed = true,
                TenantId = Guid.NewGuid()
            };
            var result = await _userManager.CreateAsync(identityUser, request.Password);
            if (!result.Succeeded)
            {
                return ServiceResponse.BadRequest("Registration Failed");
            }
            else
            {
                return ServiceResponse.Success("Registration Successful");
            }
        }
    }
}
