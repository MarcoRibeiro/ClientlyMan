using System.Linq;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Domain.Entities;
using ClientlyMan.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientlyMan.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="IPolicyRepository"/>.
/// </summary>
public class PolicyRepository : IPolicyRepository
{
    private readonly ClientlyManDbContext _dbContext;

    public PolicyRepository(ClientlyManDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Policy>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Policies
            .AsNoTracking()
            .OrderByDescending(x => x.EndDate)
            .ToListAsync(cancellationToken);
    }

    public Task<Policy?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.Policies
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Policy>> GetExpiringWithinAsync(int days, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var limit = now.AddDays(days);
        return await _dbContext.Policies
            .AsNoTracking()
            .Where(x => x.EndDate >= now && x.EndDate <= limit)
            .OrderBy(x => x.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Policy> AddAsync(Policy policy, CancellationToken cancellationToken)
    {
        _dbContext.Policies.Add(policy);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return policy;
    }

    public async Task UpdateAsync(Policy policy, CancellationToken cancellationToken)
    {
        _dbContext.Policies.Update(policy);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Policy policy, CancellationToken cancellationToken)
    {
        _dbContext.Policies.Remove(policy);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
