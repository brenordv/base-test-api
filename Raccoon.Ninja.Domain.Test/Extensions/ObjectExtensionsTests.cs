using FluentAssertions;
using Raccoon.Ninja.Domain.Extensions;

namespace Raccoon.Ninja.Domain.Test.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void TryParseDecimal_ReturnsFallbackValue_WhenValueIsNull()
    {
        // Arrange
        object value = null;
        const decimal fallbackValue = 10.5m;

        // Act
        var result = value.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(fallbackValue);
    }

    [Fact]
    public void TryParseDecimal_ReturnsFallbackValue_WhenValueIsNotConvertibleToDecimal()
    {
        // Arrange
        object value = "not a decimal";
        const decimal fallbackValue = 10.5m;

        // Act
        var result = value.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(fallbackValue);
    }

    [Theory]
    [MemberData(nameof(GetTryParseDecimalData))]
    public void TryParseDecimal_ReturnsParsedValue_WhenValueIsConvertibleToDecimal(string valueAsString,
        decimal expectedValue)
    {
        // Arrange
        object value = valueAsString;
        const decimal fallbackValue = 7.042m;

        // Act
        var result = value.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(expectedValue);
    }

    [Fact]
    public void TryParseDecimal_ReturnsFallbackValue_WhenValueIsWhitespace()
    {
        // Arrange
        object value = " ";
        const decimal fallbackValue = 10.5m;

        // Act
        var result = value.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(fallbackValue);
    }

    [Fact]
    public void TryParseDecimal_ReturnsFallbackValue_WhenValueIsEmptyString()
    {
        // Arrange
        object value = string.Empty;
        const decimal fallbackValue = 10.5m;

        // Act
        var result = value.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(fallbackValue);
    }

    [Fact]
    public void TryParseDecimal_ReturnsFallbackValue_WhenValueIsWhitespaceString()
    {
        // Arrange
        object value = "   ";
        const decimal fallbackValue = 10.5m;

        // Act
        var result = value.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(fallbackValue);
    }

    [Fact]
    public void TryParseDecimal_ReturnsParsedValue_WhenValueIsDecimal()
    {
        // Arrange
        const decimal value = 15.6m;
        object objValue = value;
        const decimal fallbackValue = 10.5m;

        // Act
        var result = objValue.TryParseDecimal(fallbackValue);

        // Assert
        result.Should().Be(value);
    }

    public static IEnumerable<object[]> GetTryParseDecimalData()
    {
        yield return new object[] { "15.6", 15.6m };
        yield return new object[] { "-15.6", -15.6m };
        yield return new object[] { "15,042.6", 15042.6m };
        yield return new object[] { "-15,042.6", -15042.6m };
        yield return new object[] { "15042.6", 15042.6m };
        yield return new object[] { "-15042.6", -15042.6m };
        yield return new object[] { "-0.0042", -0.0042m };
        yield return new object[] { "0.0042", 0.0042m };
    }
}