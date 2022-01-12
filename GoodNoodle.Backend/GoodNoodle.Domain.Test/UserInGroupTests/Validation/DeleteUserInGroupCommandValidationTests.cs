using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.UserInGroups;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.UserInGroupTests.Validation;

public class DeleteUserInGroupCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var deleteCommand = new DeleteUserInGroupCommand(Guid.Empty);
        var validator = new DeleteUserInGroupCommandValidation();
        var validationResult = validator.Validate(deleteCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInGroupIdEmpty, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
