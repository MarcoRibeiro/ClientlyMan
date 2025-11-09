using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Domain.Entities;

/// <summary>
/// Represents an insurance policy under management.
/// </summary>
public class Policy
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public required string Insurer { get; set; }
    public required string ProductType { get; set; }
    public required string PolicyNumber { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Premium { get; set; }
    public PolicyStatus Status { get; set; }
    public int RenewNotifyDays { get; set; }

    public Customer? Customer { get; set; }

    /// <summary>
    /// Determines whether the policy is expiring within the provided time window.
    /// </summary>
    public bool IsExpiringWithin(TimeSpan window)
    {
        var now = DateTime.UtcNow;
        return Status is PolicyStatus.Active or PolicyStatus.ExpiringSoon
               && EndDate <= now.Add(window)
               && EndDate >= now;
    }
}
