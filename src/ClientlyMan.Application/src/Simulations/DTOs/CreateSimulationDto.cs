using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Application.Simulations.DTOs;

/// <summary>
/// Incoming payload for creating simulations.
/// </summary>
public sealed record CreateSimulationDto(
    Guid CustomerId,
    DateTime RequestedAt,
    DateTime? SentAt,
    SimulationStatus Status,
    string? Notes
);
