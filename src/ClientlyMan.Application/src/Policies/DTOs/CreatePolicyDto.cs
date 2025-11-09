using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Application.Policies.DTOs;

/// <summary>
/// Incoming payload for creating policies.
/// </summary>
public sealed record CreatePolicyDto(
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
