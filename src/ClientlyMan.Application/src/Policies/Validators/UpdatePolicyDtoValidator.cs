using FluentValidation;

namespace ClientlyMan.Application.Policies.Validators;

/// <summary>
/// Validates update policy payloads.
/// </summary>
public class UpdatePolicyDtoValidator : AbstractValidator<UpdatePolicyDto>
{
    public UpdatePolicyDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.Insurer)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.ProductType)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.PolicyNumber)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.StartDate)
            .NotEmpty();

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate);

        RuleFor(x => x.Premium)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.RenewNotifyDays)
            .GreaterThanOrEqualTo(0);
    }
}
