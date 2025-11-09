using ClientlyMan.Domain.Enums;
using FluentValidation;

namespace ClientlyMan.Application.Policies.Validators;

/// <summary>
/// Validates create policy payloads.
/// </summary>
public class CreatePolicyDtoValidator : AbstractValidator<CreatePolicyDto>
{
    public CreatePolicyDtoValidator()
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

        RuleFor(x => x.Status)
            .IsInEnum()
            .Must(status => status is not PolicyStatus.ExpiringSoon)
            .WithMessage("Policies cannot be created directly with ExpiringSoon status.");

        RuleFor(x => x.RenewNotifyDays)
            .GreaterThanOrEqualTo(0);
    }
}
