using ClientlyMan.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientlyMan.Infrastructure.Persistence;

/// <summary>
/// EF Core database context for ClientlyMan.
/// </summary>
public class ClientlyManDbContext : DbContext
{
    public ClientlyManDbContext(DbContextOptions<ClientlyManDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<Simulation> Simulations => Set<Simulation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientlyManDbContext).Assembly);
    }
}
