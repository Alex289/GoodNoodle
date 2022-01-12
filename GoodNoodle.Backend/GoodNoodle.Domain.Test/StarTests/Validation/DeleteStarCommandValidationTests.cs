using GoodNoodle.Domain.Commands.Stars;
using GoodNoodle.Domain.Errors;
using GoodNoodle.Domain.Validations.Star;
using System;
using Xunit;

namespace GoodNoodle.Domain.Test.StarTests.Validation;

public class DeleteStarCommandValidationTests
{
    [Fact]
    public void Should_Not_Validate_Invalid_Empty_Id()
    {
        var deleteCommand = new DeleteStarCommand(Guid.Empty);
        var validator = new ValidateDeleteStar();
        var validationResult = validator.Validate(deleteCommand);

        Assert.False(validationResult.IsValid);

        Assert.Equal(DomainErrorCodes.StarEmptyId, validationResult.Errors[0].ErrorCode);

        Assert.Single(validationResult.Errors);
    }
}
