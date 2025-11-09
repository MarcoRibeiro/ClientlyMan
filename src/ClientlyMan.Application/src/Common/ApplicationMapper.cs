using ClientlyMan.Application.Customers.DTOs;
using ClientlyMan.Application.Policies.DTOs;
using ClientlyMan.Application.Simulations.DTOs;
using ClientlyMan.Domain.Entities;

namespace ClientlyMan.Application.Common;

/// <summary>
/// Centralised mapping helpers used by the application layer.
/// </summary>
public static class ApplicationMapper
{
    public static CustomerResponseDto ToDto(Customer entity) => new(
        entity.Id,
        entity.Name,
        entity.TaxNumber,
        entity.Email,
        entity.Phone,
        entity.Notes,
        entity.CreatedAt
    );

    public static PolicyResponseDto ToDto(Policy entity) => new(
        entity.Id,
        entity.CustomerId,
        entity.Insurer,
        entity.ProductType,
        entity.PolicyNumber,
        entity.StartDate,
        entity.EndDate,
        entity.Premium,
        entity.Status,
        entity.RenewNotifyDays
    );

    public static SimulationResponseDto ToDto(Simulation entity) => new(
        entity.Id,
        entity.CustomerId,
        entity.RequestedAt,
        entity.SentAt,
        entity.Status,
        entity.Notes
    );
}
