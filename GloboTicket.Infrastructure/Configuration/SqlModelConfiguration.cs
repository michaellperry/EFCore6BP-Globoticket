using GloboTicket.SharedKernel.Configuration;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.Infrastructure.Configuration;

internal class SqlModelConfiguration : IModelConfiguration
{
    public void ConfigureModel(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlModelConfiguration).Assembly);
    }
}
