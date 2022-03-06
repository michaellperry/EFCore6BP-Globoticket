using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using GloboTicket.Domain;

namespace GloboTicket.API.DesignTime;

public class GloboTicketContextFactory : IDesignTimeDbContextFactory<GloboTicketContext>
{
    private const string AdminConnectionString = "GLOBOTICKET_ADMIN_CONNECTION_STRING";

    public GloboTicketContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(AdminConnectionString);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException(
                $"Please set the environment variable {AdminConnectionString}");
        }

        var options = new DbContextOptionsBuilder<GloboTicketContext>()
            .UseSqlServer(connectionString)
            .Options;
        return new GloboTicketContext(options, new DesignTimeModelConfiguration());
    }
}
