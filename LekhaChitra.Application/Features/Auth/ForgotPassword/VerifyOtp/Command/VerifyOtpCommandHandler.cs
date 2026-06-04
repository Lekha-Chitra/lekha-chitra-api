using LekhaChitra.Application.Features.Auth.ForgotPassword.SendOtp.Command;
using LekhaChitra.Application.Helpers.InMemoryDb.EmailDb;
using LekhaChitra.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Auth.ForgotPassword.VerifyOtp.Command
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ServiceResponse>
    {
        private readonly OtpInMemoryDb _otpInMemoryDb;
        public VerifyOtpCommandHandler(OtpInMemoryDb otpInMemoryDb)
        {
            _otpInMemoryDb = otpInMemoryDb;
        }
        public async Task<ServiceResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var otpData =  _otpInMemoryDb.GetOtp(request.Email);
            if (otpData == null)
            {
                return ServiceResponse.NotFound("Invalid Otp please try again.");
            }
            else 
            {
                if (otpData.ExpiryTime < DateTime.UtcNow)
                {
                    _otpInMemoryDb.RemoveOtp(request.Email);

                    return ServiceResponse.BadRequest("OTP has expired. Please request a new one.");
                }
                if (otpData.Attempts >= 3)
                {
                    _otpInMemoryDb.RemoveOtp(request.Email);

                    return ServiceResponse.Unauthorized("Too many invalid attempts.");
                }
                if (otpData.Otp != request.otp)
                {
                    otpData.Attempts++;
                    // update cache
                    _otpInMemoryDb.UpdateOtp(request.Email, otpData);
                    return ServiceResponse.BadRequest($"Invalid OTP. Attempt {otpData.Attempts}/3");
                }

                _otpInMemoryDb.RemoveOtp(request.Email);

                return ServiceResponse.Success("OTP verified successfully.");
            }
        }
    }
}
