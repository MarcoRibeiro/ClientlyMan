using System.Linq;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Domain.Entities;
using ClientlyMan.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClientlyMan.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of <see cref="ICustomerRepository"/>.
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly ClientlyManDbContext _dbContext;

    public CustomerRepository(ClientlyManDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Customer>> SearchAsync(string? name, string? taxNumber, CancellationToken cancellationToken)
    {
        IQueryable<Customer> query = _dbContext.Customers.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            var pattern = $"%{name.Trim()}%";
            query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
        }

        if (!string.IsNullOrWhiteSpace(taxNumber))
        {
            var normalized = taxNumber.Trim();
            query = query.Where(x => x.TaxNumber == normalized);
        }

        return await query
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        _dbContext.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Policy>> GetPoliciesAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Policies
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.EndDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Simulation>> GetSimulationsAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return await _dbContext.Simulations
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.RequestedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken)
    {
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return customer;
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken)
    {
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Customer customer, CancellationToken cancellationToken)
    {
        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistsWithTaxNumberAsync(string taxNumber, Guid? excludingId, CancellationToken cancellationToken)
    {
        IQueryable<Customer> query = _dbContext.Customers;

        if (excludingId.HasValue)
        {
            query = query.Where(x => x.Id != excludingId.Value);
        }

        return query.AnyAsync(x => x.TaxNumber == taxNumber, cancellationToken);
    }
}
