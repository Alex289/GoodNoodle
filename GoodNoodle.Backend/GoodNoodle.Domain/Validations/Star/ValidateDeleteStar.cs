using FluentValidation;
using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.Star;

public class ValidateDeleteStar : AbstractValidator<DeleteStarCommand>
{
    public ValidateDeleteStar()
    {
        AddRuleForId();
    }

    private void AddRuleForId()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.StarEmptyId);
    }
}
