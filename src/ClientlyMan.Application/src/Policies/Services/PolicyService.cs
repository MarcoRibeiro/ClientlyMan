using System.Linq;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Application.Common;
using ClientlyMan.Application.Policies.DTOs;
using ClientlyMan.Domain.Entities;
using FluentValidation;

namespace ClientlyMan.Application.Policies.Services;

/// <summary>
/// Default implementation of <see cref="IPolicyService"/>.
/// </summary>
public class PolicyService : IPolicyService
{
    private readonly IPolicyRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CreatePolicyDto> _createValidator;
    private readonly IValidator<UpdatePolicyDto> _updateValidator;

    public PolicyService(
        IPolicyRepository repository,
        ICustomerRepository customerRepository,
        IValidator<CreatePolicyDto> createValidator,
        IValidator<UpdatePolicyDto> updateValidator)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<PolicyResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var policies = await _repository.GetAllAsync(cancellationToken);
        return policies.Select(ApplicationMapper.ToDto).ToList();
    }

    public async Task<PolicyResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var policy = await _repository.GetByIdAsync(id, cancellationToken);
        return policy is null ? null : ApplicationMapper.ToDto(policy);
    }

    public async Task<IReadOnlyList<PolicyResponseDto>> GetExpiringAsync(int days, CancellationToken cancellationToken)
    {
        var policies = await _repository.GetExpiringWithinAsync(days, cancellationToken);
        return policies.Select(ApplicationMapper.ToDto).ToList();
    }

    public async Task<PolicyResponseDto> CreateAsync(CreatePolicyDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);

        await EnsureCustomerExists(dto.CustomerId, cancellationToken);

        var policy = new Policy
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            Insurer = dto.Insurer,
            ProductType = dto.ProductType,
            PolicyNumber = dto.PolicyNumber,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Premium = dto.Premium,
            Status = dto.Status,
            RenewNotifyDays = dto.RenewNotifyDays
        };

        var created = await _repository.AddAsync(policy, cancellationToken);
        return ApplicationMapper.ToDto(created);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdatePolicyDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var policy = await _repository.GetByIdAsync(id, cancellationToken);
        if (policy is null)
        {
            return false;
        }

        await EnsureCustomerExists(dto.CustomerId, cancellationToken);

        policy.CustomerId = dto.CustomerId;
        policy.Insurer = dto.Insurer;
        policy.ProductType = dto.ProductType;
        policy.PolicyNumber = dto.PolicyNumber;
        policy.StartDate = dto.StartDate;
        policy.EndDate = dto.EndDate;
        policy.Premium = dto.Premium;
        policy.Status = dto.Status;
        policy.RenewNotifyDays = dto.RenewNotifyDays;

        await _repository.UpdateAsync(policy, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var policy = await _repository.GetByIdAsync(id, cancellationToken);
        if (policy is null)
        {
            return false;
        }

        await _repository.DeleteAsync(policy, cancellationToken);
        return true;
    }

    private async Task EnsureCustomerExists(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId, cancellationToken);
        if (customer is null)
        {
            throw new ValidationException($"Customer '{customerId}' does not exist.");
        }
    }
}
