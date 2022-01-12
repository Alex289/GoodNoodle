using GoodNoodle.Domain.Commands.NoodleUser;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleUser;
using Xunit;

namespace GoodNoodle.Domain.Test.NoodleUserTests.Validations;

public class ChangePasswordCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_OldPassword()
    {
        var updateCommand = new ChangePasswordCommand(
            "",
            "NewPassword123#");

        var validator = new ValidateChangePassword();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyPassword, validationResult.Errors[0].ErrorCode);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_NewPassword()
    {
        var updateCommand = new ChangePasswordCommand(
            "OldPassword123#",
            "");

        var validator = new ValidateChangePassword();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyPassword, validationResult.Errors[0].ErrorCode);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Min_Length_OldPassword()
    {
        var updateCommand = new ChangePasswordCommand(
            "Ab1#",
            "NewPassword123#");

        var validator = new ValidateChangePassword();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserShortPassword, validationResult.Errors[0].ErrorCode);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Min_Length_NewPassword()
    {
        var updateCommand = new ChangePasswordCommand(
            "OldPassword123#",
            "Ab1#");

        var validator = new ValidateChangePassword();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserShortPassword, validationResult.Errors[0].ErrorCode);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_Length_OldPassword()
    {
        var updateCommand = new ChangePasswordCommand(
            "jiEgK8vw6dOq5W7StnLpoL3mxDIJfSPXdOitK5iEvJ743BasNZ12#",
            "NewPassword123#");

        var validator = new ValidateChangePassword();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserLongPassword, validationResult.Errors[0].ErrorCode);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_Length_NewPassword()
    {
        var updateCommand = new ChangePasswordCommand(
            "OldPassword123#",
            "jiEgK8vw6dOq5W7StnLpoL3mxDIJfSPXdOitK5iEvJ743BasNZ12#");

        var validator = new ValidateChangePassword();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserLongPassword, validationResult.Errors[0].ErrorCode);
    }
}
