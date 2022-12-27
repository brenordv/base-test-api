using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Validators;

public static class GuidValidator
{
    private static (bool, string[]) TryIsValidForId(this Guid guid)
    {
        return guid == Guid.Empty 
            ? (false, new[] { "Id cannot be empty." }) 
            : (true, Array.Empty<string>());
    }

    public static void EnsureIsValidForId(this Guid guid)
    {
        var (passed, errors) = guid.TryIsValidForId();
        if (passed) return;
        throw new ValidationException($"Guid '{guid}' instance cannot be used as Id. Reasons: {string.Join(", ", errors)}");
    }
}