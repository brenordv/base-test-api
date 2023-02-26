using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Validators;

public static class EnumValidator
{
    public static void IsValidEnum<T>(this T value, string fieldName = "Enum") where T : struct, Enum
    {
        if (Enum.IsDefined(typeof(T), value)) return;

        throw new ValidationException($"{fieldName} '{value}' is not a valid value for {typeof(T).Name}.");
    }
}