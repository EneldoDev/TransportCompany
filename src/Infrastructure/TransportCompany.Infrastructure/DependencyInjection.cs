using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransportCompany.Application.Interfaces;
using TransportCompany.Infrastructure.Common.Persistance;
using TransportCompany.Infrastructure.Helpers;
using TransportCompany.Infrastructure.Trucks;

namespace TransportCompany.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddPersistance(configuration);
            return services;
        }

        private static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CompanyDbContext>(options => options.UseSqlServer(configuration.GetConnectionString(ConstValues.ConnectionStringName)));
            services.AddScoped<ITrucksRepository, TrucksRepository>();
            return services;
        }
    }
}
