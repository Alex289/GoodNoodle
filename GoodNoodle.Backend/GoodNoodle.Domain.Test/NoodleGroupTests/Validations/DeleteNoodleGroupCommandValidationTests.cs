using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleGroup;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.NoodleGroupTests.Validations;

public class DeleteNoodleGroupCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var deleteCommand = new DeleteNoodleGroupCommand(Guid.Empty);
        var validator = new ValidateDeleteNoodleGroup();
        var validationResult = validator.Validate(deleteCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.GroupEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
