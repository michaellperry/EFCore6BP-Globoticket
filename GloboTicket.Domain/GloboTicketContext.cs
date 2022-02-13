using GloboTicket.Domain.Entities;
using GloboTicket.SharedKernel.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Domain;

public class GloboTicketContext : DbContext
{
    private readonly IModelConfiguration modelConfiguration;

    public GloboTicketContext(DbContextOptions<GloboTicketContext> options, IModelConfiguration modelConfiguration) :
        base(options)
    {
        this.modelConfiguration = modelConfiguration;
    }

    public DbSet<Venue> Venue { get; set; }
    public DbSet<Act> Act { get; set; }
    public DbSet<Show> Show { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GloboTicketContext).Assembly);
        modelConfiguration.ConfigureModel(modelBuilder);
    }
}
