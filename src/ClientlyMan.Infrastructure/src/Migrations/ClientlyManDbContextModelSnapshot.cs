using System;
using ClientlyMan.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace ClientlyMan.Infrastructure.Migrations;

/// <summary>
/// Snapshot of the current EF Core model.
/// </summary>
[DbContext(typeof(ClientlyManDbContext))]
public class ClientlyManDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder.HasAnnotation("ProductVersion", "9.0.0-preview.4.24267.6");

        modelBuilder.Entity("ClientlyMan.Domain.Entities.Customer", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<DateTime>("CreatedAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(320)
                .HasColumnType("character varying(320)");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<string>("Notes")
                .HasMaxLength(2000)
                .HasColumnType("character varying(2000)");

            b.Property<string>("Phone")
                .HasMaxLength(30)
                .HasColumnType("character varying(30)");

            b.Property<string>("TaxNumber")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("character varying(50)");

            b.HasKey("Id");

            b.HasIndex("TaxNumber")
                .IsUnique();

            b.ToTable("Customers");
        });

        modelBuilder.Entity("ClientlyMan.Domain.Entities.Policy", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<Guid>("CustomerId")
                .HasColumnType("uuid");

            b.Property<DateTime>("EndDate")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Insurer")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<string>("PolicyNumber")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("character varying(100)");

            b.Property<string>("ProductType")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("character varying(200)");

            b.Property<decimal>("Premium")
                .HasColumnType("numeric(18,2)");

            b.Property<int>("RenewNotifyDays")
                .HasColumnType("integer");

            b.Property<DateTime>("StartDate")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("character varying(50)");

            b.HasKey("Id");

            b.HasIndex("CustomerId");

            b.HasIndex("PolicyNumber");

            b.ToTable("Policies");
        });

        modelBuilder.Entity("ClientlyMan.Domain.Entities.Simulation", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<Guid>("CustomerId")
                .HasColumnType("uuid");

            b.Property<string>("Notes")
                .HasMaxLength(2000)
                .HasColumnType("character varying(2000)");

            b.Property<DateTime>("RequestedAt")
                .HasColumnType("timestamp with time zone");

            b.Property<DateTime?>("SentAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("character varying(50)");

            b.HasKey("Id");

            b.HasIndex("CustomerId");

            b.ToTable("Simulations");
        });

        modelBuilder.Entity("ClientlyMan.Domain.Entities.Policy", b =>
        {
            b.HasOne("ClientlyMan.Domain.Entities.Customer", "Customer")
                .WithMany("Policies")
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Customer");
        });

        modelBuilder.Entity("ClientlyMan.Domain.Entities.Simulation", b =>
        {
            b.HasOne("ClientlyMan.Domain.Entities.Customer", "Customer")
                .WithMany("Simulations")
                .HasForeignKey("CustomerId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Customer");
        });

        modelBuilder.Entity("ClientlyMan.Domain.Entities.Customer", b =>
        {
            b.Navigation("Policies");

            b.Navigation("Simulations");
        });
#pragma warning restore 612, 618
    }
}
