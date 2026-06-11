using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Helpers.InMemoryDb.EmailDb
{
    using LekhaChitra.Application.Helpers.InMemoryDb.Email.Model;
    using Microsoft.Extensions.Logging;
    using System.Collections.Concurrent;

    public class OtpInMemoryDb
    {
        private readonly ConcurrentDictionary<string, OtpCacheModel> _otps = new();
        private readonly ILogger<OtpInMemoryDb> _logger;

        public OtpInMemoryDb(ILogger<OtpInMemoryDb> logger)
        {
            _logger = logger;
        }

        // Add or update OTP
        public void StoreOtp(string email, string otp)
        {
            var model = new OtpCacheModel
            {
                Otp = otp,
                ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                Attempts = 0,
                ResendCount = 1,
                LastSentTime = DateTime.UtcNow
            };

            _otps[email] = model;

            _logger.LogInformation($"OTP stored for {email}, expires at {model.ExpiryTime}");
        }

        public OtpCacheModel? GetOtp(string email)
        {
            _otps.TryGetValue(email, out var otp);
            return otp;
        }

       public IList<KeyValuePair<string,OtpCacheModel>> GetAllOtp()
        {
            return _otps.ToList();
        }
        public bool ValidateOtp(string email, string otp)
        {
            if (!_otps.TryGetValue(email, out var model))
                return false;

            if (model.ExpiryTime < DateTime.UtcNow)
            {
                _otps.TryRemove(email, out _);
                _logger.LogInformation($"OTP expired for {email}");
                return false;
            }

            if (model.Attempts >= 3)
            {
                _otps.TryRemove(email, out _);
                _logger.LogInformation($"OTP blocked due to max attempts for {email}");
                return false;
            }

            if (model.Otp != otp)
            {
                model.Attempts++;
                _otps[email] = model;

                _logger.LogInformation($"Invalid OTP attempt for {email}. Attempts: {model.Attempts}");
                return false;
            }

    
            _otps.TryRemove(email, out _);
            _logger.LogInformation($"OTP verified successfully for {email}");

            return true;
        }

        public void UpdateOtp(string email, OtpCacheModel model)
        {
            _otps[email] = model;
        }
        public void RemoveOtp(string email)
        {
            _otps.TryRemove(email, out _);
            _logger.LogInformation($"OTP removed for {email}");
        }

        public bool HasOtp(string email)
        {
            return _otps.ContainsKey(email);
        }
    }
}
