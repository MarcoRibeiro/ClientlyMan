using FluentValidation;

namespace ClientlyMan.Application.Customers.Validators;

/// <summary>
/// Validates incoming requests when creating customers.
/// </summary>
public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.TaxNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320);

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Notes)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes));
    }
}
