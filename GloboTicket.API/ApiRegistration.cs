using GloboTicket.API.Commands;

namespace GloboTicket.API;

public static class ApiRegistration
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddTransient<RescheduleShowCommand>();

        return services;
    }
}
