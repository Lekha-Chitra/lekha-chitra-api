using LekhaChitra.Application.Helpers.InMemoryDb.EmailDb;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.BackgroundServices
{
    public class UpdateOtpCacheBackgroundServices : BackgroundService
    {
        private readonly OtpInMemoryDb _otpInMemoryDb;
        private readonly ILogger<UpdateOtpCacheBackgroundServices> _logger;
        public UpdateOtpCacheBackgroundServices(OtpInMemoryDb otpInMemoryDb,
        ILogger<UpdateOtpCacheBackgroundServices> logger)
        {
            _otpInMemoryDb = otpInMemoryDb;
            _logger = logger;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateOtpCache(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            }
        }

        public async Task UpdateOtpCache(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            foreach (var item in _otpInMemoryDb.GetAllOtp())
            {
                var email = item.Key;
                var otp = item.Value;

                if (otp.BlockedUntil.HasValue &&
                    otp.BlockedUntil <= now)
                {
                    _otpInMemoryDb.RemoveOtp(email);
                    _logger.LogInformation(
                         "OTP block removed for {Email}",
                         email);

                }

            }
        }
    }
}
