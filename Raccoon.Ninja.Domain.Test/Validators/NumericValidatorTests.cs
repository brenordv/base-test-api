using FluentAssertions;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Test.Validators;

public class NumericValidatorTests
{
    [Theory]
    [InlineData(10, 5)]
    [InlineData(5, 5)]
    public void IsGreaterThanOrEqualTo_ShouldPassForValidNumbers(decimal value, decimal minValue)
    {
        // Arrange
        const string fieldName = "TestNumber";
        
        // Act & Assert
        // assert that the action does not throw an exception
        var action = () => value.IsGreaterThanOrEqualTo(minValue, fieldName);
        action.Should().NotThrow<ValidationException>();
    }

    [Theory]
    [InlineData(2, 5)]
    public void IsGreaterThanOrEqualTo_ShouldThrowForInvalidNumbers(decimal value, decimal minValue)
    {
        // Arrange
        const string fieldName = "TestNumber";
        
        // Act & Assert
        // assert that the action throws a ValidationException with the correct error message
        var action = () => value.IsGreaterThanOrEqualTo(minValue, fieldName);
        action.Should().Throw<ValidationException>()
            .WithMessage($"{fieldName} cannot be lesser than {minValue}.");
    }
}
