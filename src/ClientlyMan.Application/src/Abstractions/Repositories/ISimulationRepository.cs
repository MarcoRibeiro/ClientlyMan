using ClientlyMan.Domain.Entities;

namespace ClientlyMan.Application.Abstractions.Repositories;

/// <summary>
/// Persistence operations for simulations.
/// </summary>
public interface ISimulationRepository
{
    Task<IReadOnlyList<Simulation>> GetAllAsync(CancellationToken cancellationToken);
    Task<Simulation?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Simulation> AddAsync(Simulation simulation, CancellationToken cancellationToken);
    Task UpdateAsync(Simulation simulation, CancellationToken cancellationToken);
    Task DeleteAsync(Simulation simulation, CancellationToken cancellationToken);
}
