using System.Linq;
using ClientlyMan.Application.Abstractions.Repositories;
using ClientlyMan.Application.Common;
using ClientlyMan.Application.Simulations.DTOs;
using ClientlyMan.Domain.Entities;
using FluentValidation;

namespace ClientlyMan.Application.Simulations.Services;

/// <summary>
/// Default implementation of <see cref="ISimulationService"/>.
/// </summary>
public class SimulationService : ISimulationService
{
    private readonly ISimulationRepository _repository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CreateSimulationDto> _createValidator;
    private readonly IValidator<UpdateSimulationDto> _updateValidator;

    public SimulationService(
        ISimulationRepository repository,
        ICustomerRepository customerRepository,
        IValidator<CreateSimulationDto> createValidator,
        IValidator<UpdateSimulationDto> updateValidator)
    {
        _repository = repository;
        _customerRepository = customerRepository;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    public async Task<IReadOnlyList<SimulationResponseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var simulations = await _repository.GetAllAsync(cancellationToken);
        return simulations.Select(ApplicationMapper.ToDto).ToList();
    }

    public async Task<SimulationResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var simulation = await _repository.GetByIdAsync(id, cancellationToken);
        return simulation is null ? null : ApplicationMapper.ToDto(simulation);
    }

    public async Task<SimulationResponseDto> CreateAsync(CreateSimulationDto dto, CancellationToken cancellationToken)
    {
        await _createValidator.ValidateAndThrowAsync(dto, cancellationToken);
        await EnsureCustomerExists(dto.CustomerId, cancellationToken);

        var simulation = new Simulation
        {
            Id = Guid.NewGuid(),
            CustomerId = dto.CustomerId,
            RequestedAt = dto.RequestedAt,
            SentAt = dto.SentAt,
            Status = dto.Status,
            Notes = dto.Notes
        };

        var created = await _repository.AddAsync(simulation, cancellationToken);
        return ApplicationMapper.ToDto(created);
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateSimulationDto dto, CancellationToken cancellationToken)
    {
        await _updateValidator.ValidateAndThrowAsync(dto, cancellationToken);

        var simulation = await _repository.GetByIdAsync(id, cancellationToken);
        if (simulation is null)
        {
            return false;
        }

        await EnsureCustomerExists(dto.CustomerId, cancellationToken);

        simulation.CustomerId = dto.CustomerId;
        simulation.RequestedAt = dto.RequestedAt;
        simulation.SentAt = dto.SentAt;
        simulation.Status = dto.Status;
        simulation.Notes = dto.Notes;

        await _repository.UpdateAsync(simulation, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var simulation = await _repository.GetByIdAsync(id, cancellationToken);
        if (simulation is null)
        {
            return false;
        }

        await _repository.DeleteAsync(simulation, cancellationToken);
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
