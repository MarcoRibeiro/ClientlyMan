using System;
using System.Linq;
using ClientlyMan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClientlyMan.Tests.Integration.Infrastructure;

/// <summary>
/// Custom factory overriding infrastructure dependencies for tests.
/// </summary>
public class ClientlyManApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ClientlyManDbContext>));
            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ClientlyManDbContext>(options =>
                options.UseInMemoryDatabase($"ClientlyManTests_{Guid.NewGuid()}"));

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ClientlyManDbContext>();
            db.Database.EnsureCreated();
        });
    }
}
