using ClientlyMan.Domain.Entities;

namespace ClientlyMan.Application.Abstractions.Repositories;

/// <summary>
/// Persistence operations for <see cref="Customer"/> aggregates.
/// </summary>
public interface ICustomerRepository
{
    Task<IReadOnlyList<Customer>> SearchAsync(string? name, string? taxNumber, CancellationToken cancellationToken);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Policy>> GetPoliciesAsync(Guid customerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Simulation>> GetSimulationsAsync(Guid customerId, CancellationToken cancellationToken);
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken);
    Task DeleteAsync(Customer customer, CancellationToken cancellationToken);
    Task<bool> ExistsWithTaxNumberAsync(string taxNumber, Guid? excludingId, CancellationToken cancellationToken);
}
