using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.Constants.Templates
{
    public static class EmailTemplates
    {
        public static string OtpEmail(string userName, string otp)
        {
            return $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
            </head>

            <body style='margin:0; padding:0; font-family:Arial, Helvetica, sans-serif; background-color:#f4f6f8;'>

                <div style='max-width:600px; margin:40px auto; background:#ffffff; border-radius:10px; overflow:hidden; box-shadow:0 2px 10px rgba(0,0,0,0.08);'>

                    <!-- Header -->
                    <div style='background:#4f46e5; padding:20px; text-align:center; color:white;'>
                        <h2 style='margin:0;'>LekhaChitra</h2>
                        <p style='margin:5px 0 0; font-size:14px;'>Secure Verification Code</p>
                    </div>

                    <!-- Body -->
                    <div style='padding:30px; color:#333;'>

                        <p style='font-size:16px;'>Hello <strong>{userName ?? "User"}</strong>,</p>

                        <p style='font-size:15px; line-height:1.6;'>
                            We received a request to reset your password. Please use the verification code below to proceed.
                        </p>

                        <!-- OTP Box -->
                        <div style='margin:30px 0; text-align:center;'>
                            <div style='display:inline-block; padding:15px 30px; font-size:32px; letter-spacing:8px;
                                        background:#f1f5f9; border:1px dashed #cbd5e1; border-radius:8px;
                                        font-weight:bold; color:#111827;'>
                                {otp}
                            </div>
                        </div>

                        <p style='font-size:14px; color:#555;'>
                            ⏳ This code is valid for <strong>10 minutes</strong>.
                        </p>

                        <p style='font-size:14px; color:#555;'>
                            If you did not request this password reset, you can safely ignore this email.
                        </p>

                    </div>

                    <!-- Footer -->
                    <div style='padding:20px; text-align:center; font-size:12px; color:#888; background:#f9fafb;'>
                        This is an automated message from <strong>LekhaChitra</strong>. Please do not reply.
                    </div>

                </div>

            </body>
            </html>";
        }
    }
}
