using FluentAssertions;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Validators;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Domain.Test.Validators;

public class ProductValidatorTests
{
    [Fact]
    public void EnsureIsValidForInsert_GivenNullProduct_ThrowsException()
    {
        Product product = null;
        var action = () => product.EnsureIsValidForInsert();
        action
            .Should()
            .Throw<EntityActionException>()
            .WithMessage("This instance cannot be added. Reasons: Cannot insert a null product.");
    }

    [Fact]
    public void EnsureIsValidForInsert_GivenProductWithId_ThrowsException()
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Version = 1
        };

        var action = () => product.EnsureIsValidForInsert();
        action
            .Should()
            .Throw<EntityActionException>()
            .WithMessage(
                $"This instance cannot be added. Reasons: This instance of product already has an Id. Should be empty.");
    }

    [Fact]
    public void EnsureIsValidForInsert_GivenProductWithOddVersion_ThrowsException()
    {
        var product = ProductGenerator.Generate(false);
        product = product with { Version = product.Version + 1 };

        var action = () => product.EnsureIsValidForInsert();
        action
            .Should()
            .Throw<EntityActionException>()
            .WithMessage(
                $"This instance cannot be added. Reasons: Even tough this is a new instance of product version is odd. Should be 1.");
    }

    [Fact]
    public void EnsureIsValidForInsert_GivenValidProduct_DoesNotThrowException()
    {
        var product = ProductGenerator.Generate(false, true);

        var action = () => product.EnsureIsValidForInsert();
        action.Should().NotThrow<Exception>();
    }
}