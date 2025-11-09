using System.Reflection;
using ClientlyMan.Application.Customers.Services;
using ClientlyMan.Application.Policies.Services;
using ClientlyMan.Application.Simulations.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ClientlyMan.Application.Common;

/// <summary>
/// Configures application-layer services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IPolicyService, PolicyService>();
        services.AddScoped<ISimulationService, SimulationService>();

        return services;
    }
}
