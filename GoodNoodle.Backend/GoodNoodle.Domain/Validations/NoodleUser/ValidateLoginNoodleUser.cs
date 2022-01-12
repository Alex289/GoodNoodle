using FluentValidation;
using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.NoodleUser;

public class ValidateLoginNoodleUser : AbstractValidator<LoginNoodleUserCommand>
{
    public ValidateLoginNoodleUser()
    {
        AddRuleForEmail();
        AddRuleForPassword();
    }

    private void AddRuleForEmail()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyEmail);
    }

    private void AddRuleForPassword()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyPassword);
    }
}
