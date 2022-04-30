using GloboTicket.API.Commands;
using GloboTicket.API.Queries;

namespace GloboTicket.API;

public static class ApiRegistration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddTransient<ListShowsQuery>();

        services.AddTransient<RescheduleShowCommand>();

        return services;
    }
}
