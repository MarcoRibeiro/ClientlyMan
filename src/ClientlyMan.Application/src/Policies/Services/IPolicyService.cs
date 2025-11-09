using ClientlyMan.Application.Policies.DTOs;

namespace ClientlyMan.Application.Policies.Services;

/// <summary>
/// Application boundary for working with policies.
/// </summary>
public interface IPolicyService
{
    Task<IReadOnlyList<PolicyResponseDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<PolicyResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<PolicyResponseDto>> GetExpiringAsync(int days, CancellationToken cancellationToken);
    Task<PolicyResponseDto> CreateAsync(CreatePolicyDto dto, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Guid id, UpdatePolicyDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
