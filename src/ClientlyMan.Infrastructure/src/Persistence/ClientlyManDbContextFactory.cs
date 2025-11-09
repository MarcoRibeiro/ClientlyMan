using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ClientlyMan.Infrastructure.Persistence;

/// <summary>
/// Design-time factory enabling EF Core tooling.
/// </summary>
public class ClientlyManDbContextFactory : IDesignTimeDbContextFactory<ClientlyManDbContext>
{
    public ClientlyManDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ClientlyManDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=clientlyman;Username=postgres;Password=postgres");
        return new ClientlyManDbContext(optionsBuilder.Options);
    }
}
