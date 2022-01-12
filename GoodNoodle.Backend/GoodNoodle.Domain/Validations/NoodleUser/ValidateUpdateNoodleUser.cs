using FluentValidation;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.NoodleUser;

public class ValidateUpdateNoodleUser : AbstractValidator<UpdateNoodleUserCommand>
{

    public ValidateUpdateNoodleUser()
    {
        AddRuleForId();
        AddRuleForEmail();
        AddRuleForFullName();
    }

    private void AddRuleForEmail()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyEmail)
            .EmailAddress()
            .WithErrorCode(DomainErrorCodes.UserInvalidEmail);
    }

    private void AddRuleForFullName()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyFullName)
            .MaximumLength(100)
            .WithErrorCode(DomainErrorCodes.UserLongFullName);
    }

    private void AddRuleForId()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyId);
    }
}
