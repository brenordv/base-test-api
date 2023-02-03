using FluentAssertions;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Test.Validators;

public class GuidValidatorTests
{
    [Fact]
    public void EnsureIsValidForId_WithValidGuid_ShouldNotThrowException()
    {
        // Arrange
        var value = Guid.NewGuid();

        // Act
        var action = () => value.EnsureIsValidForId();

        // Assert
        action.Should().NotThrow<ValidationException>();
    }

    [Fact]
    public void EnsureIsValidForId_WithEmptyGuid_ShouldThrowException()
    {
        // Arrange
        var value = Guid.Empty;

        // Act
        var action = () => value.EnsureIsValidForId();

        // Assert
        action.Should().Throw<ValidationException>()
            .WithMessage(
                "Guid '00000000-0000-0000-0000-000000000000' instance cannot be used as Id. Reasons: Id cannot be empty.");
    }

    [Fact]
    public void EnsureIsValidForId_WithEmptyGuidAndCustomErrorMessage_ShouldThrowExceptionWithCustomMessage()
    {
        // Arrange
        var value = Guid.Empty;
        const string errorMessage = "Custom error message.";

        // Act
        var action = () => value.EnsureIsValidForId(errorMessage);

        // Assert
        action.Should().Throw<ValidationException>()
            .WithMessage(errorMessage);
    }
}