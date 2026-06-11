using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
namespace LekhaChitra.API.Startup
{
    public static class JwtServiceRegistration
    {
        public static IServiceCollection AddJwtServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;

                    // only for dev false
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidAudience = configuration["JWT:ValidAudience"],

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JWT:Secret"])
                        ),

                        ClockSkew = TimeSpan.Zero // IMPORTANT: removes 5 min token delay issues
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // ✅ 1. PRIORITY: Cookie-based JWT (your current approach)
                            if (context.Request.Cookies.TryGetValue("MyAuthValue", out var cookieToken))
                            {
                                context.Token = cookieToken;
                                return Task.CompletedTask;
                            }

                            // (Optional) 2. Fallback: Authorization header (for Postman/Swagger flexibility)
                            var authHeader = context.Request.Headers.Authorization.ToString();
                            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                            {
                                context.Token = authHeader["Bearer ".Length..].Trim();
                                return Task.CompletedTask;
                            }

                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var response = new ProblemDetails
                            {
                                Status = 401,
                                Title = "Authentication Failed",
                                Detail = context.Exception.Message
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        },

                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            var response = new ProblemDetails
                            {
                                Status = 401,
                                Title = "Unauthorized",
                                Detail = "You are not authorized to access this resource."
                            };

                            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        }
                    };
                });

            return services;
        }
    }
}
