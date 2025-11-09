using ClientlyMan.Domain.Enums;

namespace ClientlyMan.Application.Policies.DTOs;

/// <summary>
/// API response representation of a policy.
/// </summary>
public sealed record PolicyResponseDto(
    Guid Id,
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
