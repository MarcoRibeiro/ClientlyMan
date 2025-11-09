using System;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Infrastructure.Persistence;
using ClientlyMan.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClientlyMan.Infrastructure;

/// <summary>
/// Registers infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ClientlyMan")
                               ?? configuration["ConnectionStrings:ClientlyMan"]
                               ?? throw new InvalidOperationException("The ClientlyMan connection string was not configured.");

        services.AddDbContext<ClientlyManDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IPolicyRepository, PolicyRepository>();
        services.AddScoped<ISimulationRepository, SimulationRepository>();

        return services;
    }
}
