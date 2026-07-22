using Polly;
using Polly.Retry;
using System.Net.Mail;

namespace LekhaChitra.API.Extensions.ResiliencePolicy
{
    public static class ResilienceServiceExtensions
    {
        public static IServiceCollection AddApplicationResilience(
         this IServiceCollection services)
        {
            services.AddResiliencePipeline(
               "smtp-email",
               (pipelineBuilder, context) =>
               {
                   var loggerFactory = context.ServiceProvider.GetRequiredService<ILoggerFactory>();

                   var logger = loggerFactory.CreateLogger("SMTP Resilience");

                   pipelineBuilder.AddRetry(
                       new RetryStrategyOptions
                       {
                           MaxRetryAttempts = 3,
                           Delay = TimeSpan.FromSeconds(2),
                           BackoffType =DelayBackoffType.Exponential,
                           UseJitter = true,
                           ShouldHandle = new PredicateBuilder()
                                           .Handle<SmtpException>(SmtpResilienceHelper.IsTransientSmtpException)
                                           .Handle<TimeoutException>(),

                           OnRetry = args =>
                           {
                               logger.LogWarning(
                                   args.Outcome.Exception,
                                   "SMTP email failed. " +
                                   "Retry attempt {RetryAttempt} " +
                                   "will occur after {RetryDelay} seconds.",
                                   args.AttemptNumber + 1,
                                   args.RetryDelay.TotalSeconds);

                               return default;
                           }
                        });
                });
            
            return services;
        }
    }
}
