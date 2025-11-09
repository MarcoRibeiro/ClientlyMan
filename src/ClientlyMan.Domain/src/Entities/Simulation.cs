using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Domain.Entities;

/// <summary>
/// Represents an insurance simulation/quote request.
/// </summary>
public class Simulation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public SimulationStatus Status { get; set; }
    public string? Notes { get; set; }

    public Customer? Customer { get; set; }
}
