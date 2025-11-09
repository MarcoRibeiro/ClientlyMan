using ClientlyMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientlyMan.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures EF Core mapping for simulations.
/// </summary>
public class SimulationConfiguration : IEntityTypeConfiguration<Simulation>
{
    public void Configure(EntityTypeBuilder<Simulation> builder)
    {
        builder.ToTable("Simulations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);
    }
}
