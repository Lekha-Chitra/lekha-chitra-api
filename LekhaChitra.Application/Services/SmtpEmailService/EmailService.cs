using Azure.Core.Pipeline;
using LekhaChitra.Application.DTO.Email;
using LekhaChitra.Application.Helpers.InMemoryDb.EmailDb;
using LekhaChitra.Application.Interfaces.SmtpEmailService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Registry;
using Polly.Retry;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace LekhaChitra.Application.Services.SmtpEmail
{
    public class EmailService : IEmailService
    {
        private readonly SmtpEmailSettingDTO _settings;
        private readonly ResiliencePipeline _resiliencePipeline;
        public EmailService(IOptions<SmtpEmailSettingDTO> options,
                            ResiliencePipelineProvider<string> pipelineProvider)
        {
            _settings = options.Value;
            _resiliencePipeline = pipelineProvider.GetPipeline("smtp-email");
        }

        public string GenerateOtp()
        {
            return RandomNumberGenerator
                  .GetInt32(100000, 1000000)
                  .ToString();
        }

        public async Task SendAsync(string to, string subject, string body)
        {

            await _resiliencePipeline.ExecuteAsync(
            async cancellationToken =>
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

                 await client.SendMailAsync(mail, cancellationToken);

            });
        }
    }
}
