using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Auth.ForgotPassword.ResetPassword.Command
{
    public record ResetPasswordCommand(string Email, string NewPassword) : IRequest<ServiceResponse>;
}
