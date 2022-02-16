using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Domain;
public class GloboTicketContext : DbContext
{
    public GloboTicketContext(DbContextOptions<GloboTicketContext> options) :
        base(options)
    {
    }

    public DbSet<Venue> Venue { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GloboTicketContext).Assembly);
    }
}
