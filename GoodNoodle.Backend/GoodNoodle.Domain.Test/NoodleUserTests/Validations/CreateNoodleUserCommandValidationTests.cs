using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleUser;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.NoodleUserTests.Validations;

public class CreateNoodleUserCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Email()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "TestPassword123#",
            "");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyEmail, validationResult.Errors[0].ErrorCode);

        Assert.Equal(2, validationResult.Errors.Count);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Email()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "TestPassword123#",
            "tolle email");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserInvalidEmail, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.Empty,
            "Test name",
            "TestPassword123#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Password()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyPassword, validationResult.Errors[0].ErrorCode);
        Assert.Equal(DomainErrorCodes.UserShortPassword, validationResult.Errors[1].ErrorCode);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Min_Password_Length()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "tI4#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserShortPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_Password_Length()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "0rM9vd3b6No5mwMhex9aFlU7sy5fKRpFtV47ifwnO820ChaUMzj#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserLongPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_No_Uppercase_Password()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "abc123#abc123#abc123#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserUppercaseLetterPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_No_Lowercase_Password()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "ABC123#ABC123#ABC123#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserLowercaseLetterPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_No_Number_Password()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "abcA#abcA#abcA#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserNumberPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_No_Special_Character_Password()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "Test name",
            "abc123Aabc123Aabc123A",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserSpecialCharPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_FullName()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "",
            "TestPassword123#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyFullName, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_FullName_Length()
    {
        var createCommand = new CreateNoodleUserCommand(
            Guid.NewGuid(),
            "qjVAVHeLvdHDHJvPRiLyrtF7ut1zYNhx0nPjGVkNuyOgPvz5EdyqjVAVHeLvdHDHJvPRiLyrtF7ut1zYNhx0nPjGVkNuyOgPvz5Edy",
            "TestPassword123#",
            "email@provider.com");
        var validator = new ValidateCreateNoodleUser();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserLongFullName, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
