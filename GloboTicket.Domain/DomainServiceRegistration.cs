using GloboTicket.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.Domain;

public static class DomainServiceRegistration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<PromotionService>();
        services.AddScoped<SalesService>();
        services.AddScoped<FeedService>();
        return services;
    }
}
