using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.UserInGroups;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.UserInGroupTests.Validation;

public class CreateUserInGroupCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var createCommand = new CreateUserInGroupCommand(Guid.Empty, Guid.NewGuid());
        var validator = new CreateUserInGroupCommandValidation();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.InvitationEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_UserInGroupGroup_Id()
    {
        var createCommand = new CreateUserInGroupCommand(Guid.NewGuid(), Guid.Empty);
        var validator = new CreateUserInGroupCommandValidation();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInGroupIdEmpty, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
