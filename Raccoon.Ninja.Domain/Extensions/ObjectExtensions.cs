using System.Globalization;

namespace Raccoon.Ninja.Domain.Extensions;

public static class ObjectExtensions
{
    private static readonly CultureInfo CultureInfo = new("en-US");

    public static decimal TryParseDecimal(this object value, decimal fallbackValue)
    {
        if (value == null) return fallbackValue;

        if (value is decimal alreadyADecimal) return alreadyADecimal;

        var valueAsString = value.ToString();

        return !string.IsNullOrWhiteSpace(valueAsString)
               && decimal.TryParse(valueAsString, NumberStyles.Number, CultureInfo, out var parsedValue)
            ? parsedValue
            : fallbackValue;
    }
}