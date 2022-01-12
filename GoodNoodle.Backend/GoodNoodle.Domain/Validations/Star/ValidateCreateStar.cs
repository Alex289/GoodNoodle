using FluentValidation;
using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.Star;

public class ValidateCreateStar : AbstractValidator<CreateStarCommand>
{
    public ValidateCreateStar()
    {
        AddRuleForId();
        AddRuleForUserId();
        AddRuleForGroupId();
        AddRuleForReason();
    }

    private void AddRuleForId()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.StarEmptyId);
    }

    private void AddRuleForUserId()
    {
        RuleFor(x => x.NoodleUserId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.StarEmptyUserId);
    }

    private void AddRuleForGroupId()
    {
        RuleFor(x => x.NoodleGroupId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.StarEmptyGroupId);
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
