using FluentAssertions;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Test.Validators;

public class EnumValidatorTests
{
    [Fact]
    public void IsValidEnum_WithValidEnumValue_ShouldNotThrowException()
    {
        // Arrange
        var value = TestEnum.Value1;

        // Act
        var action = () => value.IsValidEnum();

        // Assert
        action.Should().NotThrow<ValidationException>();
    }

    [Fact]
    public void IsValidEnum_WithInvalidEnumValue_ShouldThrowException()
    {
        // Arrange
        var value = (TestEnum)999;

        // Act
        var action = () => value.IsValidEnum();

        // Assert
        action.Should().Throw<ValidationException>()
            .WithMessage("Enum '999' is not a valid value for TestEnum.");
    }

    private enum TestEnum
    {
        Value1,
        Value2
    }
}