using FluentValidation;
using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Errors;

namespace GoodNoodle.Domain.Validations.NoodleGroup;

public class ValidateDeleteNoodleGroup : AbstractValidator<DeleteNoodleGroupCommand>
{
    public ValidateDeleteNoodleGroup()
    {
        AddRuleForGroupId();
    }

    protected void AddRuleForGroupId()
    {
        RuleFor(cmd => cmd.Id)
            .NotEmpty()
            .WithErrorCode(DomainErrorCodes.GroupEmptyId);
    }
}
