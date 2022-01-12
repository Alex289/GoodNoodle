using FluentValidation;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.NoodleUser;

public class ValidateDeleteNoodleUser : AbstractValidator<DeleteNoodleUserCommand>
{
    public ValidateDeleteNoodleUser()
    {
        AddRuleForId();
    }

    private void AddRuleForId()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyId);
    }
}
