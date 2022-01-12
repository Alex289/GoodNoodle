using GoodNoodle.Domain.Commands;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleUser;
using Xunit;

namespace GoodNoodle.Domain.Test.NoodleUserTests.Validations;

public class LoginNoodleUserCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Email()
    {
        var loginCommand = new LoginNoodleUserCommand("", "Tolles Passwort");
        var validator = new ValidateLoginNoodleUser();
        var validationResult = validator.Validate(loginCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyEmail, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Password()
    {
        var loginCommand = new LoginNoodleUserCommand("email@provider.com", "");
        var validator = new ValidateLoginNoodleUser();
        var validationResult = validator.Validate(loginCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.UserEmptyPassword, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
