using Bogus;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Test.Helpers.Generators;

public static class AddProductModelGenerator
{
    private static readonly Faker<AddProductModel> _fakerNoId = Init();

    public static IEnumerable<AddProductModel> Generate(int qty)
    {
        for (var i = 0; i < qty; i++)
        {
            yield return Generate();
        }
    }

    public static AddProductModel Generate()
    {
        var baseProduct = _fakerNoId.Generate();
        var product = new AddProductModel
        {
            Company = baseProduct.Company,
            Description = baseProduct.Description,
            Name = baseProduct.Name,
            Tier = baseProduct.Tier,
            SuggestedPrice = baseProduct.SuggestedPrice
        };

        return product;
    }

    private static Faker<AddProductModel> Init()
    {
        var faker = new Faker<AddProductModel>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.SuggestedPrice, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(p => p.Tier, f => f.PickRandom<ProductTier>())
            .RuleFor(p => p.Company, f => f.Company.CompanyName());

        return faker;
    }
}