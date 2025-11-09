using System.Linq;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Domain.Entities;
using ClientlyMan.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientlyMan.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ISimulationRepository"/>.
/// </summary>
public class SimulationRepository : ISimulationRepository
{
    private readonly ClientlyManDbContext _dbContext;

    public SimulationRepository(ClientlyManDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Simulation>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Simulations
            .AsNoTracking()
            .OrderByDescending(x => x.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public Task<Simulation?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.Simulations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<Simulation> AddAsync(Simulation simulation, CancellationToken cancellationToken)
    {
        _dbContext.Simulations.Add(simulation);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return simulation;
    }

    public async Task UpdateAsync(Simulation simulation, CancellationToken cancellationToken)
    {
        _dbContext.Simulations.Update(simulation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Simulation simulation, CancellationToken cancellationToken)
    {
        _dbContext.Simulations.Remove(simulation);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
