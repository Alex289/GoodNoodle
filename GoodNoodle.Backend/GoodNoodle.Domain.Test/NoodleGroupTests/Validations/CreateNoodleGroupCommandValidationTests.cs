using GoodNoodle.Domain.Commands.NoodleGroup;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.NoodleGroup;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.NoodleGroupTests.Validations;

public class CreateNoodleGroupCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var createCommand = new CreateNoodleGroupCommand(Guid.Empty, "Test name", "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=");
        var validator = new ValidateCreateNoodleGroup();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.GroupEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Name()
    {
        var createCommand = new CreateNoodleGroupCommand(Guid.NewGuid(), "", "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=");
        var validator = new ValidateCreateNoodleGroup();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.GroupEmptyName, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Max_Name_Length()
    {
        var createCommand = new CreateNoodleGroupCommand(Guid.NewGuid(), "zvJuhpFZsIhJESB9OO89YR659zK5k6sRnzNlMgII1PNmTHyPTED", "7PuAiwHl9OzqyuB0AXTSSO+fBVg3iZGddNT8BCLeOzY=");
        var validator = new ValidateCreateNoodleGroup();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.GroupTooLongName, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }

    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Image()
    {
        var createCommand = new CreateNoodleGroupCommand(Guid.NewGuid(), "Test name", "hello");
        var validator = new ValidateCreateNoodleGroup();
        var validationResult = validator.Validate(createCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.GroupImageNotBase64, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
