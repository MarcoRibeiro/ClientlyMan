using ClientlyMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientlyMan.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures EF Core mapping for policies.
/// </summary>
public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.ToTable("Policies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Insurer)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ProductType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.PolicyNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Premium)
            .HasColumnType("numeric(18,2)");

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.RenewNotifyDays)
            .IsRequired();

        builder.HasIndex(x => x.PolicyNumber);
    }
}
