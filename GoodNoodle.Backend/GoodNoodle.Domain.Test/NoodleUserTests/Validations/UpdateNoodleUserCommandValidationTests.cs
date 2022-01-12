using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleUser;
using System;
using Xunit;
using static GoodNoodle.Domain.Enums;

namespace GoodNoodle.Domain.Test.NoodleUserTests.Validations;

public class UpdateNoodleUserCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Email()
    {
        var updateCommand = new UpdateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            UserStatus.Pending,
            "",
            UserRole.User);

        var validator = new ValidateUpdateNoodleUser();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyEmail, validationResult.Errors[0].ErrorCode);

        Assert.Equal(2, validationResult.Errors.Count);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Email()
    {
        var updateCommand = new UpdateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            UserStatus.Pending,
            "test email",
            UserRole.User);

        var validator = new ValidateUpdateNoodleUser();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInvalidEmail, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var updateCommand = new UpdateNoodleUserCommand(
            Guid.Empty,
            "Test name",
            UserStatus.Pending,
            "email@provider.com",
            UserRole.User);

        var validator = new ValidateUpdateNoodleUser();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_FullName()
    {
        var updateCommand = new UpdateNoodleUserCommand(
            Guid.NewGuid(),
            "",
            UserStatus.Pending,
            "email@provider.com",
            UserRole.User);

        var validator = new ValidateUpdateNoodleUser();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyFullName, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_LastName_Length()
    {
        var updateCommand = new UpdateNoodleUserCommand(
            Guid.NewGuid(),
            "qjVAVHeLvdHDHJvPRiLyrtF7ut1zYNhx0nPjGVkNuyOgPvz5EdyqjVAVHeLvdHDHJvPRiLyrtF7ut1zYNhx0nPjGVkNuyOgPvz5Edy",
            UserStatus.Pending,
            "email@provider.com",
            UserRole.User);

        var validator = new ValidateUpdateNoodleUser();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserLongFullName, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
