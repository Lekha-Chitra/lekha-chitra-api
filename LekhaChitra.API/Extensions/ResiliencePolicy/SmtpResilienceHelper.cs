using System.Net.Mail;

namespace LekhaChitra.API.Extensions.ResiliencePolicy
{
    public static class SmtpResilienceHelper
    {
        public static bool IsTransientSmtpException(
            SmtpException exception)
        {
            return exception.StatusCode switch
            {
                SmtpStatusCode.ServiceNotAvailable => true,

                SmtpStatusCode.MailboxBusy => true,

                SmtpStatusCode.InsufficientStorage => true,

                _ => false
            };
        }
    }
    
}
