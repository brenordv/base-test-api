using Bogus;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Test.Helpers.Generators;

public static class ProductGenerator
{
    private static readonly Faker<Product> FakerFull = Init(true);
    private static readonly Faker<Product> FakerNoId = Init(false);

    public static IEnumerable<Product> Generate(int qty, bool withId = true)
    {
        for (var i = 0; i < qty; i++)
        {
            yield return Generate(withId);
        }
    }

    public static Product Generate(bool includeId = true, bool resetVersion = false)
    {
        if (!resetVersion)
            return includeId ? FakerFull.Generate() : FakerNoId.Generate();

        var baseProduct = FakerNoId.Generate();
        var product = new Product
        {
            Company = baseProduct.Company,
            Description = baseProduct.Description,
            Name = baseProduct.Name,
            Tier = baseProduct.Tier,
            CreatedAt = baseProduct.CreatedAt,
            ModifiedAt = baseProduct.ModifiedAt,
            SuggestedPrice = baseProduct.SuggestedPrice,
            Version = 1 // Since version cannot be set back to 1, we need to create a new instance.
        };

        return includeId ? product with { Id = Guid.NewGuid() } : product;
    }

    private static Faker<Product> Init(bool includeId)
    {
        var faker = new Faker<Product>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Company, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.CreatedAt, f => DateTime.Now.AddDays(f.PickRandom(-90, 0)))
            .RuleFor(p => p.ModifiedAt, f => DateTime.Now.AddDays(f.PickRandom(-90, 0)))
            .RuleFor(p => p.SuggestedPrice, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(p => p.Tier, f => f.PickRandom<ProductTier>())
            .RuleFor(p => p.Version, f => f.PickRandom(1, 42));

        if (includeId)
            faker.RuleFor(p => p.Id, f => Guid.NewGuid());

        return faker;
    }
}