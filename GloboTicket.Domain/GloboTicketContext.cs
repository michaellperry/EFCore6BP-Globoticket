using GloboTicket.SharedKernel.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Domain;

public class GloboTicketContext : DbContext
{
    private readonly IModelConfiguration modelConfiguration;

    public GloboTicketContext(DbContextOptions options, IModelConfiguration modelConfiguration) :
        base(options)
    {
        this.modelConfiguration = modelConfiguration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GloboTicketContext).Assembly);
        modelConfiguration.ConfigureModel(modelBuilder);
    }
}
