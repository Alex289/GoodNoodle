using FluentValidation;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.UserInGroups;

public class UpdateUserInGroupCommandValidation : AbstractValidator<UpdateUserInGroupCommand>
{
    public UpdateUserInGroupCommandValidation()
    {
        AddRuleForId();
        AddRuleForNoodleGroupId();
        AddRuleForNoodleUserId();
        AddRuleForRole();
    }

    protected void AddRuleForId()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupIdEmpty);
    }

    protected void AddRuleForNoodleGroupId()
    {
        RuleFor(cmd => cmd.NoodleGroupId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupGroupIdEmpty);
    }

    protected void AddRuleForNoodleUserId()
    {
        RuleFor(cmd => cmd.NoodleUserId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupUserIdEmpty);
    }

    protected void AddRuleForRole()
    {
        RuleFor(cmd => cmd.Role)
            .IsInEnum()
            .WithErrorCode(DomainErrorCodes.UserInGroupRoleNotExist);
    }
}
