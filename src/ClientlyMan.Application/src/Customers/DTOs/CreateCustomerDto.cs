namespace ClientlyMan.Application.Customers.DTOs;

/// <summary>
/// Incoming payload for creating a customer.
/// </summary>
public sealed record CreateCustomerDto(
    string Name,
    string TaxNumber,
    string Email,
    string? Phone,
    string? Notes
);
