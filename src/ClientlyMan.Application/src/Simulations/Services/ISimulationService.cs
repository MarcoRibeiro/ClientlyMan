using ClientlyMan.Application.Simulations.DTOs;

namespace ClientlyMan.Application.Simulations.Services;

/// <summary>
/// Application boundary for simulations.
/// </summary>
public interface ISimulationService
{
    Task<IReadOnlyList<SimulationResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<SimulationResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<SimulationResponseDto> CreateAsync(CreateSimulationDto dto, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid id, UpdateSimulationDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
