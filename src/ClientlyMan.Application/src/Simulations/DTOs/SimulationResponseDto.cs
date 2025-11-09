using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Application.Simulations.DTOs;

/// <summary>
/// API response representation of a simulation.
/// </summary>
public sealed record SimulationResponseDto(
    Guid Id,
    Guid CustomerId,
    DateTime RequestedAt,
    DateTime? SentAt,
    SimulationStatus Status,
    string? Notes
);
