using GloboTicket.Domain;
using GloboTicket.Infrastructure.Configuration;
using GloboTicket.SharedKernel.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentException("Connection string cannot be null");
        }
        
        services.AddSingleton<IModelConfiguration, SqlModelConfiguration>();
        services.AddDbContext<GloboTicketContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(
                    typeof(ServiceRegistration).Assembly.FullName);
                sqlOptions.UseNetTopologySuite();
            });
        });
        services.AddDomain();

        return services;
    }
}
