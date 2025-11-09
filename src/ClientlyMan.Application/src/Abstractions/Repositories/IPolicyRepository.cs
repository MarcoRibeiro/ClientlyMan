using ClientlyMan.Domain.Entities;

namespace ClientlyMan.Application.Abstractions.Repositories;

/// <summary>
/// Persistence operations for policies.
/// </summary>
public interface IPolicyRepository
{
    Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken);
    Task<Policy?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Policy>> GetExpiringWithinAsync(int days, CancellationToken cancellationToken);
    Task<Policy> AddAsync(Policy policy, CancellationToken cancellationToken);
    Task UpdateAsync(Policy policy, CancellationToken cancellationToken);
    Task DeleteAsync(Policy policy, CancellationToken cancellationToken);
}
