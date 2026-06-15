using LekhaChitra.Application.DTO.Email;
using LekhaChitra.Application.Helpers.InMemoryDb.EmailDb;
using LekhaChitra.Application.Interfaces.SmtpEmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace LekhaChitra.Application.Services.SmtpEmail
{
    public class EmailService : IEmailService
    {
        private readonly SmtpEmailSettingDTO _settings;
       

        public EmailService(IOptions<SmtpEmailSettingDTO> options)
        {
            _settings = options.Value;
        }

        public string GenerateOtp()
        {
            return RandomNumberGenerator
                  .GetInt32(100000, 1000000)
                  .ToString();
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_settings.Host)
            {
                Port = _settings.Port,
                Credentials = new NetworkCredential(_settings.User, _settings.Password),
                EnableSsl = _settings.EnableSsl
            };

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            };

            mail.To.Add(to);

            await client.SendMailAsync(mail);

          
        }
    }
}
