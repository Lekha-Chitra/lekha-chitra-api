using LekhaChitra.Application.Helpers.BackgroundServices;
using LekhaChitra.Application.Helpers.InMemoryDb.EmailDb;
using LekhaChitra.Application.Helpers.JwtHelper;
using LekhaChitra.Application.Helpers.TenantService;
using LekhaChitra.Application.Interfaces.SmtpEmailService;
using LekhaChitra.Application.Services.SmtpEmail;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LekhaChitra.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

            services.AddHostedService<UpdateOtpCacheBackgroundServices>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<OtpInMemoryDb>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ITenantService, TenantService>();

            return services;
        }
    }
}
