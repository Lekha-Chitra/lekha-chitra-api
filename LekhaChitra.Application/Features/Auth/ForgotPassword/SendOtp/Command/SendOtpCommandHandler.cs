using LekhaChitra.Application.Constants.Templates;
using LekhaChitra.Application.Helpers.InMemoryDb.EmailDb;
using LekhaChitra.Application.Interfaces.SmtpEmailService;
using LekhaChitra.Application.Response;
using LekhaChitra.Domain.Entities.Application.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Features.Auth.ForgotPassword.SendOtp.Command
{
    public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, ServiceResponse>
    {
        private readonly IEmailService _emailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OtpInMemoryDb _otpInMemoryDb;

        public SendOtpCommandHandler(
            IEmailService emailService,
            UserManager<ApplicationUser> userManager,
                OtpInMemoryDb otpInMemoryDb
                    )
        {
            _emailService = emailService;
            _userManager = userManager;
            _otpInMemoryDb = otpInMemoryDb;
        }
        public async Task<ServiceResponse> Handle(SendOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return ServiceResponse.NotFound("User with the provided email does not exist.");

            var existingOtp = _otpInMemoryDb.GetOtp(request.Email);

            // =========================
            // CASE 1: FIRST TIME OTP
            // =========================
            if (existingOtp == null)
            {
                var otp = _emailService.GenerateOtp();

                if (string.IsNullOrEmpty(otp))
                    return ServiceResponse.InternalServerError("Failed to generate OTP.");

                var body = EmailTemplates.OtpEmail(user.UserName, otp);

                await _emailService.SendAsync(
                    user.Email,
                    "LekhaChitra Verification Code",
                    body
                );

                _otpInMemoryDb.StoreOtp(user.Email, otp);

                return ServiceResponse.Success("OTP sent successfully.");
            }

            if (existingOtp.BlockedUntil != null &&
                existingOtp.BlockedUntil > DateTime.UtcNow)
            {
                return ServiceResponse.Unauthorized(
                    "You have reached maximum resend limit. Please try again later.");
            }

            if (existingOtp.ExpiryTime < DateTime.UtcNow)
            {
                var otp = _emailService.GenerateOtp();

                _otpInMemoryDb.StoreOtp(request.Email, otp);

                var body = EmailTemplates.OtpEmail(user.UserName, otp);

                await _emailService.SendAsync(
                    request.Email,
                    "LekhaChitra Verification Code",
                    body
                );

                return ServiceResponse.Success("OTP expired. New OTP sent.");
            }


            if ((DateTime.UtcNow - existingOtp.LastSentTime).TotalSeconds < 30)
            {
                return ServiceResponse.Unauthorized("Please wait before requesting another OTP.");
            }

            if (existingOtp.ResendCount >= 3)
            {
                existingOtp.BlockedUntil = DateTime.UtcNow.AddMinutes(10);

                _otpInMemoryDb.UpdateOtp(request.Email, existingOtp);

                return ServiceResponse.Unauthorized(
                    "Maximum resend limit reached. Please try again after 10 minutes.");
            }


            var newOtp = _emailService.GenerateOtp();

            if (string.IsNullOrEmpty(newOtp))
                return ServiceResponse.InternalServerError("Failed to generate OTP.");

            existingOtp.Otp = newOtp;
            existingOtp.Attempts = 0;
            existingOtp.ResendCount++;
            existingOtp.LastSentTime = DateTime.UtcNow;
            existingOtp.ExpiryTime = DateTime.UtcNow.AddMinutes(10);

            _otpInMemoryDb.UpdateOtp(request.Email, existingOtp);

            var resendBody = EmailTemplates.OtpEmail(user.UserName, newOtp);

            await _emailService.SendAsync(
                request.Email,
                "LekhaChitra Verification Code",
                resendBody
            );

            return ServiceResponse.Success("OTP resent successfully.");
        }
    }
}
