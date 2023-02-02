using FluentAssertions;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Test.Validators;

public class DateTimeValidatorTests
{
    [Fact]
    public void IsInThePast_WithDateInThePast_ShouldNotThrowException()
    {
        // Arrange
        var date = DateTime.UtcNow.AddDays(-1);

        // Act
        var action= () => date.IsInThePast();

        // Assert
        action.Should().NotThrow<ValidationException>();
    }

    [Fact]
    public void IsInThePast_WithDateInTheFuture_ShouldThrowException()
    {
        // Arrange
        var date = DateTime.UtcNow.AddDays(1);

        // Act
        var action= () => date.IsInThePast();

        // Assert
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Date cannot be in the future.");
    }

    [Fact]
    public void IsInThePast_WithMinValue_ShouldThrowException()
    {
        // Arrange
        var date = DateTime.MinValue;

        // Act
        var action= () => date.IsInThePast();

        // Assert
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Date cannot be equal to minimum date.");
    }

    [Fact]
    public void IsInThePast_WithMaxValue_ShouldThrowException()
    {
        // Arrange
        var date = DateTime.MaxValue;

        // Act
        var action = () => date.IsInThePast();

        // Assert
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Date cannot be equal to maximum date.");
    }
}