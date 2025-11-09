using System.Linq;
using ClientlyMan.Application.Customers.DTOs;
using ClientlyMan.Application.Customers.Services;
using ClientlyMan.Application.Policies.DTOs;
using ClientlyMan.Application.Simulations.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ClientlyMan.Api.Controllers;

/// <summary>
/// Manages customer resources.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Lists customers with optional filtering.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CustomerResponseDto>>> GetAsync([FromQuery] string? name, [FromQuery] string? taxNumber, CancellationToken cancellationToken)
    {
        var customers = await _customerService.SearchAsync(name, taxNumber, cancellationToken);
        return Ok(customers);
    }

    /// <summary>
    /// Retrieves a single customer.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetByIdAsync(id, cancellationToken);
        return customer is null ? NotFound() : Ok(customer);
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CustomerResponseDto>> CreateAsync([FromBody] CreateCustomerDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _customerService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(ex);
        }
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCustomerDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _customerService.UpdateAsync(id, dto, cancellationToken);
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
    /// Deletes a customer.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _customerService.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves policies linked to a customer.
    /// </summary>
    [HttpGet("{id:guid}/policies")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PolicyResponseDto>>> GetPoliciesAsync(Guid id, CancellationToken cancellationToken)
    {
        var policies = await _customerService.GetPoliciesAsync(id, cancellationToken);
        return Ok(policies);
    }

    /// <summary>
    /// Retrieves simulations linked to a customer.
    /// </summary>
    [HttpGet("{id:guid}/simulations")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SimulationResponseDto>>> GetSimulationsAsync(Guid id, CancellationToken cancellationToken)
    {
        var simulations = await _customerService.GetSimulationsAsync(id, cancellationToken);
        return Ok(simulations);
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
