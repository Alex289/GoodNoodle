using FluentValidation;
using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Extensions.Validations;

namespace GoodNoodle.Domain.Validations.NoodleGroup;

public class ValidateUpdateNoodleGroup : AbstractValidator<UpdateNoodleGroupCommand>
{
    public ValidateUpdateNoodleGroup()
    {
        AddRuleForGroupId();
        AddRuleForName();
        AddRuleForImage();
    }

    protected void AddRuleForGroupId()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.GroupEmptyId);
    }

    protected void AddRuleForName()
    {
        RuleFor(cmd => cmd.Name)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.GroupEmptyName)
            .MaximumLength(50)
            .WithErrorCode(DomainErrorCodes.GroupTooLongName);
    }

    protected void AddRuleForImage()
    {
        RuleFor(cmd => cmd.Image)
            .StringMustBeBase64()
            .WithErrorCode(DomainErrorCodes.GroupImageNotBase64);
    }
}
