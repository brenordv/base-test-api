using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Validators;

public static class ProductValidator
{
    private static (bool, string[]) TryIsValidForInsert(this Product product)
    {
        var errors = new List<string>();
        if (product == null)
            return (false, new[] { $"Cannot insert a null {nameof(product)}." });


        if (product.Id != Guid.Empty)
            errors.Add($"This instance of {nameof(product)} already has an Id. Should be empty.");

        if (product.Version != 1)
            errors.Add($"Even tough this is a new instance of {nameof(product)} version is odd. Should be 1.");

        return (!errors.Any(), errors.ToArray());
    }

    public static void EnsureIsValidForInsert(this Product product)
    {
        var (passed, errors) = product.TryIsValidForInsert();
        if (passed) return;
        throw new EntityActionException($"This instance cannot be added. Reasons: {string.Join(", ", errors)}");
    }
}