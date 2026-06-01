using LekhaChitra.Application.Interfaces.Data;
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
 

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
