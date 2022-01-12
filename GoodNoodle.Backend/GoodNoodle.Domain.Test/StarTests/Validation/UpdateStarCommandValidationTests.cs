using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.Star;
using System;
using System.Linq;
using Xunit;

namespace GoodNoodle.Domain.Test.StarTests.Validation;

public class UpdateStarCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var updateCommand = new UpdateStarCommand(Guid.Empty, "Legit reason");
        var validator = new ValidateUpdateStar();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Reason()
    {
        var updateCommand = new UpdateStarCommand(Guid.NewGuid(), "");
        var validator = new ValidateUpdateStar();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyReason, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_Reason_Length()
    {
        var updateCommand = new UpdateStarCommand(Guid.NewGuid(), string.Concat(Enumerable.Repeat("ab", 1001)));
        var validator = new ValidateUpdateStar();
        var validationResult = validator.Validate(updateCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarLongReason, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
