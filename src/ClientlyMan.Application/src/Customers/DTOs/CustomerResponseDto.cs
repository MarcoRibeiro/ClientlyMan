namespace ClientlyMan.Application.Customers.DTOs;

/// <summary>
/// Representation of a customer sent back to API clients.
/// </summary>
public sealed record CustomerResponseDto(
    Guid Id,
    string Name,
    string TaxNumber,
    string Email,
    string? Phone,
    string? Notes,
    DateTime CreatedAt
);
