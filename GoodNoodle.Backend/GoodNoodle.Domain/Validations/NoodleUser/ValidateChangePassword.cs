using FluentValidation;
using GoodNoodle.Domain.Commands.NoodleUser;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.NoodleUser;

public class ValidateChangePassword : AbstractValidator<ChangePasswordCommand>
{
    public ValidateChangePassword()
    {
        AddRuleForOldPassword();
        AddRuleForNewPassword();
    }

    private void AddRuleForOldPassword()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyPassword)
            .MinimumLength(6)
            .WithErrorCode(DomainErrorCodes.UserShortPassword)
            .MaximumLength(50)
            .WithErrorCode(DomainErrorCodes.UserLongPassword);
    }

    private void AddRuleForNewPassword()
    {
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.UserEmptyPassword)
            .MinimumLength(6)
            .WithErrorCode(DomainErrorCodes.UserShortPassword)
            .MaximumLength(50)
            .WithErrorCode(DomainErrorCodes.UserLongPassword);
    }
}
