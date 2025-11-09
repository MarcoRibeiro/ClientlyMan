namespace ClientlyMan.Domain.Entities;

/// <summary>
/// Represents an insurance customer.
/// </summary>
public class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string TaxNumber { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Policy> Policies { get; set; } = new List<Policy>();
    public ICollection<Simulation> Simulations { get; set; } = new List<Simulation>();

    /// <summary>
    /// Adds a policy to the customer and ensures bidirectional navigation is kept in sync.
    /// </summary>
    public void AddPolicy(Policy policy)
    {
        policy.Customer = this;
        Policies.Add(policy);
    }

    /// <summary>
    /// Adds a simulation to the customer and ensures bidirectional navigation is kept in sync.
    /// </summary>
    public void AddSimulation(Simulation simulation)
    {
        simulation.Customer = this;
        Simulations.Add(simulation);
    }
}
