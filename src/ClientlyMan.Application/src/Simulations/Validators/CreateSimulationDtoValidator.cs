using FluentValidation;

namespace ClientlyMan.Application.Simulations.Validators;

/// <summary>
/// Validates simulation creation payloads.
/// </summary>
public class CreateSimulationDtoValidator : AbstractValidator<CreateSimulationDto>
{
    public CreateSimulationDtoValidator()
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
