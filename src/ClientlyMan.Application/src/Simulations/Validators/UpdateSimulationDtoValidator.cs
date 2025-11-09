using FluentValidation;

namespace ClientlyMan.Application.Simulations.Validators;

/// <summary>
/// Validates simulation update payloads.
/// </summary>
public class UpdateSimulationDtoValidator : AbstractValidator<UpdateSimulationDto>
{
    public UpdateSimulationDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.RequestedAt)
            .LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5));

        RuleFor(x => x.Status)
            .IsInEnum();

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
