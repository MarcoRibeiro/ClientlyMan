using System.Linq;
using ClientlyMan.Application.Simulations.DTOs;
using ClientlyMan.Application.Simulations.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ClientlyMan.Api.Controllers;

/// <summary>
/// Manages simulation resources.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SimulationsController : ControllerBase
{
    private readonly ISimulationService _simulationService;

    public SimulationsController(ISimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    /// <summary>
    /// Lists all simulations.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<SimulationResponseDto>>> GetAsync(CancellationToken cancellationToken)
    {
        var simulations = await _simulationService.GetAllAsync(cancellationToken);
        return Ok(simulations);
    }

    /// <summary>
    /// Retrieves a simulation by identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SimulationResponseDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var simulation = await _simulationService.GetByIdAsync(id, cancellationToken);
        return simulation is null ? NotFound() : Ok(simulation);
    }

    /// <summary>
    /// Creates a new simulation.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SimulationResponseDto>> CreateAsync([FromBody] CreateSimulationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _simulationService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }
        catch (ValidationException ex)
        {
            return ValidationProblem(ex);
        }
    }

    /// <summary>
    /// Updates a simulation.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateSimulationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _simulationService.UpdateAsync(id, dto, cancellationToken);
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
    /// Deletes a simulation.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _simulationService.DeleteAsync(id, cancellationToken);
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
