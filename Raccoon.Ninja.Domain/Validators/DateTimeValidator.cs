using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Validators;

public static class DateTimeValidator
{
    public static void IsInThePast(this DateTime value, string fieldName = "Date")
    {
        if (value == DateTime.MinValue)
            throw new ValidationException($"{fieldName} cannot be equal to minimum date.");

        if (value == DateTime.MaxValue)
            throw new ValidationException($"{fieldName} cannot be equal to maximum date.");

        if (value.ToUniversalTime() <= DateTime.UtcNow) return;

        throw new ValidationException($"{fieldName} cannot be in the future.");
    }
}