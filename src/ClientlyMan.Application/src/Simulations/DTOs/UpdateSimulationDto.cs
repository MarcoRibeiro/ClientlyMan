using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Application.Simulations.DTOs;

/// <summary>
/// Incoming payload for updating simulations.
/// </summary>
public sealed record UpdateSimulationDto(
    Guid CustomerId,
    DateTime RequestedAt,
    DateTime? SentAt,
    SimulationStatus Status,
    string? Notes
);
