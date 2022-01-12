using FluentValidation;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.UserInGroups;

public class CreateUserInGroupCommandValidation : AbstractValidator<CreateUserInGroupCommand>
{
    public CreateUserInGroupCommandValidation()
    {
        AddRuleForInvitationId();
        AddRuleForUserInGroupId();
    }

    protected void AddRuleForInvitationId()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.InvitationEmptyId);
    }

    protected void AddRuleForUserInGroupId()
    {
        RuleFor(cmd => cmd.UserInGroupId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupIdEmpty);
    }
}
