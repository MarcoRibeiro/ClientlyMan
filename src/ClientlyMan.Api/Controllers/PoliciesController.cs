using System.Linq;
using ClientlyMan.Application.Policies.DTOs;
using ClientlyMan.Application.Policies.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ClientlyMan.Api.Controllers;

/// <summary>
/// Manages policy resources.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PoliciesController : ControllerBase
{
    private readonly IPolicyService _policyService;

    public PoliciesController(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    /// <summary>
    /// Lists all policies.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PolicyResponseDto>>> GetAsync(CancellationToken cancellationToken)
    {
        var policies = await _policyService.GetAllAsync(cancellationToken);
        return Ok(policies);
    }

    /// <summary>
    /// Retrieves a single policy.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PolicyResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var policy = await _policyService.GetByIdAsync(id, cancellationToken);
        return policy is null ? NotFound() : Ok(policy);
    }

    /// <summary>
    /// Returns policies expiring within the provided number of days.
    /// </summary>
    [HttpGet("expiring")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PolicyResponseDto>>> GetExpiringAsync([FromQuery] int days = 30, CancellationToken cancellationToken = default)
    {
        if (days <= 0)
        {
            return BadRequest("Days parameter must be greater than zero.");
        }

        var policies = await _policyService.GetExpiringAsync(days, cancellationToken);
        return Ok(policies);
    }

    /// <summary>
    /// Creates a new policy.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PolicyResponseDto>> CreateAsync([FromBody] CreatePolicyDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _policyService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(ex);
        }
    }

    /// <summary>
    /// Updates a policy.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdatePolicyDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _policyService.UpdateAsync(id, dto, cancellationToken);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(ex);
        }
    }

    /// <summary>
    /// Deletes a policy.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _policyService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    private ActionResult ValidationProblem(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

        return ValidationProblem(new ValidationProblemDetails(errors)
        {
            Status = StatusCodes.Status400BadRequest
        });
    }
}
