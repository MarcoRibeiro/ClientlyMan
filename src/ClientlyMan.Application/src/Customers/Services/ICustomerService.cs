using ClientlyMan.Application.Customers.DTOs;
using ClientlyMan.Application.Policies.DTOs;
using ClientlyMan.Application.Simulations.DTOs;

namespace ClientlyMan.Application.Customers.Services;

/// <summary>
/// Application boundary for working with customers.
/// </summary>
public interface ICustomerService
{
    Task<IReadOnlyList<CustomerResponseDto>> SearchAsync(string? name, string? taxNumber, CancellationToken cancellationToken);
    Task<CustomerResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<PolicyResponseDto>> GetPoliciesAsync(Guid customerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<SimulationResponseDto>> GetSimulationsAsync(Guid customerId, CancellationToken cancellationToken);
    Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
