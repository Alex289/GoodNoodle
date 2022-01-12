using FluentValidation;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.UserInGroups;

public class DeleteUserInGroupCommandValidation : AbstractValidator<DeleteUserInGroupCommand>
{
    public DeleteUserInGroupCommandValidation()
    {
        AddRuleForId();
    }

    protected void AddRuleForId()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupIdEmpty);
    }
}
