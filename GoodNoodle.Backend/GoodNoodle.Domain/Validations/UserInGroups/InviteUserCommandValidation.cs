using FluentValidation;
using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.UserInGroups;

public class InviteUserCommandValidation : AbstractValidator<InviteUserCommand>
{
    public InviteUserCommandValidation()
    {
        AddRuleForId();
        AddRuleForFullName();
        AddRuleForEmail();
        AddRuleForRole();
        AddRuleForGroupId();
        AddRuleForGroupName();
    }

    protected void AddRuleForId()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupIdEmpty);
    }

    private void AddRuleForFullName()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyFullName)
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserLongFullName);
    }

    private void AddRuleForEmail()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyEmail)
            .EmailAddress()
            .WithErrorCode(DomainErrorCodes.UserInvalidEmail);
    }

    protected void AddRuleForRole()
    {
        RuleFor(cmd => cmd.Role)
            .IsInEnum()
            .WithErrorCode(DomainErrorCodes.UserInGroupRoleNotExist);
    }

    protected void AddRuleForGroupId()
    {
        RuleFor(cmd => cmd.GroupId)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserInGroupIdEmpty);
    }

    private void AddRuleForGroupName()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyFullName)
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserLongFullName);
    }
}
