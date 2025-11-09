namespace ClientlyMan.Domain.Enums;

/// <summary>
/// Represents the progress of an insurance quotation simulation.
/// </summary>
public enum SimulationStatus
{
    Requested = 0,
    Sent = 1,
    Accepted = 2,
    Lost = 3
}
