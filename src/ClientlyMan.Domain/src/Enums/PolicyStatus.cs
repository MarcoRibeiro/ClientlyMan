namespace ClientlyMan.Domain.Enums;

/// <summary>
/// Represents lifecycle states for an insurance policy.
/// </summary>
public enum PolicyStatus
{
    Active = 0,
    Cancelled = 1,
    ExpiringSoon = 2,
    Expired = 3
}
