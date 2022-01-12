using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.Star;
using System;
using System.Linq;
using Xunit;

namespace GoodNoodle.Domain.Test.StarTests.Validation;

public class CreateStarCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var createCommand = new CreateStarCommand(Guid.Empty, Guid.NewGuid(), Guid.NewGuid(), "Legit reason");
        var validator = new ValidateCreateStar();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_NoodleUserId()
    {
        var createCommand = new CreateStarCommand(Guid.NewGuid(), Guid.Empty, Guid.NewGuid(), "Legit reason");
        var validator = new ValidateCreateStar();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyUserId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_NoodleGroupId()
    {
        var createCommand = new CreateStarCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.Empty, "Legit reason");
        var validator = new ValidateCreateStar();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyGroupId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Reason()
    {
        var createCommand = new CreateStarCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "");
        var validator = new ValidateCreateStar();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyReason, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_Reason_Length()
    {
        var createCommand = new CreateStarCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), string.Concat(Enumerable.Repeat("ab", 1001)));
        var validator = new ValidateCreateStar();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarLongReason, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
