using ClientlyMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClientlyMan.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures EF Core mapping for customers.
/// </summary>
public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.TaxNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.TaxNumber)
            .IsUnique();

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(320);

        builder.Property(x => x.Phone)
            .HasMaxLength(30);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasMany(x => x.Policies)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Simulations)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
