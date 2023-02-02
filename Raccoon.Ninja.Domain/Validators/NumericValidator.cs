using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Validators;

public static class NumericValidator
{
    public static void IsGreaterThanOrEqualTo<T>(this T value, T minValue, string fieldName = "Number")
        where T :  IComparable<T>
    {
        
        if (value.CompareTo(minValue) >= 0) return;
        
        throw new ValidationException($"{fieldName} cannot be lesser than {minValue}.");
    }
}