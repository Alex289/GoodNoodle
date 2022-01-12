using GoodNoodle.Domain.Commands.UserInGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.UserInGroups;
using System;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.UserInGroupTests.Validation;

public class UpdateUserInGroupCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var updateCommand = new UpdateUserInGroupCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), GroupRole.Student);
        var validator = new UpdateUserInGroupCommandValidation();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInGroupIdEmpty, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_NoodleGroup_Id()
    {
        var updateCommand = new UpdateUserInGroupCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, GroupRole.Student);
        var validator = new UpdateUserInGroupCommandValidation();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInGroupGroupIdEmpty, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_NoodleUser_Id()
    {
        var updateCommand = new UpdateUserInGroupCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), GroupRole.Student);
        var validator = new UpdateUserInGroupCommandValidation();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInGroupUserIdEmpty, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
