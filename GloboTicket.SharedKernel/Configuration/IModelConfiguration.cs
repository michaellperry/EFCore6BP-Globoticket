using Microsoft.EntityFrameworkCore;

namespace GloboTicket.SharedKernel.Configuration;

public interface IModelConfiguration
{
    void ConfigureModel(ModelBuilder modelBuilder);
}
