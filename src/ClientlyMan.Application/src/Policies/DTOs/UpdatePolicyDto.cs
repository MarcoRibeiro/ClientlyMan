using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Application.Policies.DTOs;

/// <summary>
/// Incoming payload for updating policies.
/// </summary>
public sealed record UpdatePolicyDto(
    Guid CustomerId,
    string Insurer,
    string ProductType,
    string PolicyNumber,
    DateTime StartDate,
    DateTime EndDate,
    decimal Premium,
    PolicyStatus Status,
    int RenewNotifyDays
);
