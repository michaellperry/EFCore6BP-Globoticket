using GloboTicket.API.Queries;

namespace GloboTicket.API;

public static class ApiRegistration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<ListShowsQuery>();

        return services;
    }
}
