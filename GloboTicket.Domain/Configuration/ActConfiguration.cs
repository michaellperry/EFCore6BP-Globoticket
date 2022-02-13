using GloboTicket.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GloboTicket.Domain.Configuration;

internal class ActConfiguration : IEntityTypeConfiguration<Act>
{
    public void Configure(EntityTypeBuilder<Act> builder)
    {
        builder
            .HasAlternateKey(a => a.ActGuid);
        builder
            .Property(a => a.Name)
            .HasMaxLength(100);
    }
}
