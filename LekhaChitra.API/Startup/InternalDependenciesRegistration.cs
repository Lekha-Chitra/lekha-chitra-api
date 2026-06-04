using LekhaChitra.Application.DTO;
using LekhaChitra.Application.Interfaces.Data;
using LekhaChitra.Application.Interfaces.SmtpEmailService;
using LekhaChitra.Application.Services.SmtpEmail;
using LekhaChitra.Infrastructure.Persistence.Repository;

namespace LekhaChitra.API.Startup
{
    public static class InternalDependenciesRegistration
    {
        public static void AddInternalDependencies(
           this IServiceCollection services,
           IConfiguration configuration
            )
        {
            services.AddJwtServices(configuration);
            services.AddIdentityServices();
            services.Configure<SmtpEmailSettingDTO>(configuration.GetSection("Smtp"));
       
            services.AddScoped<IUnitOfWork, UnitOfWork>();
 
        }
    }
}
