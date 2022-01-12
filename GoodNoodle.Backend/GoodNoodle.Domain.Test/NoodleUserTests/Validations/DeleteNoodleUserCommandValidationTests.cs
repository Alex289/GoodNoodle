using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleUser;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.NoodleUserTests.Validations;

public class DeleteNoodleUserCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var deleteCommand = new DeleteNoodleUserCommand(Guid.Empty);
        var validator = new ValidateDeleteNoodleUser();
        var validationResult = validator.Validate(deleteCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
