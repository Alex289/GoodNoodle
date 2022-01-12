using FluentValidation;
using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.Star;

public class ValidateUpdateStar : AbstractValidator<UpdateStarCommand>
{
    public ValidateUpdateStar()
    {
        AddRuleForId();
        AddRuleForReason();
    }

    private void AddRuleForId()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.StarEmptyId);
    }

    private void AddRuleForReason()
    {
        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.StarEmptyReason)
            .MaximumLength(1000)
            .WithErrorCode(DomainErrorCodes.StarLongReason);
    }
}
