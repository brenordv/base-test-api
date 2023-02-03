using FluentAssertions;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Test.Validators;

public class StringValidatorTests
{
    [Theory]
    [InlineData("a string with more than 10 characters", 10, "Text")]
    [InlineData("a longer string", 5, "Text")]
    public void IsTextUpToChars_ShouldThrowException_WhenTextLengthExceedsMaxChars(string text, int maxChars,
        string fieldName)
    {
        var action = () => text.IsTextUpToChars(maxChars, fieldName);
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"{fieldName} cannot exceed {maxChars} characters.");
    }

    [Theory]
    [InlineData(null, 10, "Text")]
    [InlineData("", 5, "Text")]
    [InlineData("  ", 5, "Text")]
    public void IsTextUpToChars_ShouldThrowException_WhenTextIsNullOrEmptyOrWhiteSpace(string text, int maxChars,
        string fieldName)
    {
        var action = () => text.IsTextUpToChars(maxChars, fieldName);
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"{fieldName} cannot be null, empty or empty space.");
    }

    [Theory]
    [InlineData("text", 4)]
    [InlineData("text", 10)]
    public void IsTextUpToChars_ShouldNotThrowException_WhenConditionsAreMet(string text, int maxChars)
    {
        var action = () => text.IsTextUpToChars(maxChars);
        action.Should().NotThrow<Exception>();
    }
}