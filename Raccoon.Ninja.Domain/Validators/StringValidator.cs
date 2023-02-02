using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Validators;

public static class StringValidator
{
    public static void IsTextUpToChars(this string text, int maxChars, string fieldName = "Text")
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ValidationException($"{fieldName} cannot be null, empty or empty space.");

        if (text.Length <= maxChars) return;

        throw new ValidationException($"{fieldName} cannot exceed {maxChars} characters.");
    }
}