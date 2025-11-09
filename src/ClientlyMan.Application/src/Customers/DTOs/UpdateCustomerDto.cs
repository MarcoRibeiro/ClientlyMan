namespace ClientlyMan.Application.Customers.DTOs;

/// <summary>
/// Incoming payload for updating a customer.
/// </summary>
public sealed record UpdateCustomerDto(
    string Name,
    string TaxNumber,
    string Email,
    string? Phone,
    string? Notes
);
