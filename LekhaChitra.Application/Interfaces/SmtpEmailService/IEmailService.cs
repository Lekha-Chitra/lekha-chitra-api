using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Interfaces.SmtpEmailService
{
    public interface IEmailService
    {
        string GenerateOtp();
        Task SendAsync(string to, string subject, string body);
    }
}
