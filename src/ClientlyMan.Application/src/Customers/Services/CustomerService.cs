using System.Linq;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Application.Common;
using ClientlyMan.Application.Customers.DTOs;
using ClientlyMan.Application.Policies.DTOs;
using ClientlyMan.Application.Simulations.DTOs;
using ClientlyMan.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;

namespace ClientlyMan.Application.Customers.Services;

/// <summary>
/// Default implementation of <see cref="ICustomerService"/>.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomerService(
        ICustomerRepository repository,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator)
    {
        _repository = repository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<CustomerResponseDto>> SearchAsync(string? name, string? taxNumber, CancellationToken cancellationToken)
    {
        var customers = await _repository.SearchAsync(name, taxNumber, cancellationToken);
        return customers.Select(ApplicationMapper.ToDto).ToList();
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(id, cancellationToken);
        return customer is null ? null : ApplicationMapper.ToDto(customer);
    }

    public async Task<IReadOnlyList<PolicyResponseDto>> GetPoliciesAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var policies = await _repository.GetPoliciesAsync(customerId, cancellationToken);
        return policies.Select(ApplicationMapper.ToDto).ToList();
    }

    public async Task<IReadOnlyList<SimulationResponseDto>> GetSimulationsAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var simulations = await _repository.GetSimulationsAsync(customerId, cancellationToken);
        return simulations.Select(ApplicationMapper.ToDto).ToList();
    }

    public async Task<CustomerResponseDto> CreateAsync(CreateCustomerDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);

        if (await _repository.ExistsWithTaxNumberAsync(dto.TaxNumber, null, cancellationToken))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(dto.TaxNumber), "A customer with the provided tax number already exists.")
            });
        }

        var entity = new Customer
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            TaxNumber = dto.TaxNumber,
            Email = dto.Email,
            Phone = dto.Phone,
            Notes = dto.Notes,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.AddAsync(entity, cancellationToken);
        return ApplicationMapper.ToDto(created);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        if (await _repository.ExistsWithTaxNumberAsync(dto.TaxNumber, id, cancellationToken))
        {
            throw new ValidationException(new[]
            {
                new ValidationFailure(nameof(dto.TaxNumber), "A different customer already uses the provided tax number.")
            });
        }

        existing.Name = dto.Name;
        existing.TaxNumber = dto.TaxNumber;
        existing.Email = dto.Email;
        existing.Phone = dto.Phone;
        existing.Notes = dto.Notes;

        await _repository.UpdateAsync(existing, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(id, cancellationToken);
        if (existing is null)
        {
            return false;
        }

        await _repository.DeleteAsync(existing, cancellationToken);
        return true;
    }
}
