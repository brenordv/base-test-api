using Bogus;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Test.Helpers.Generators;

public static class ProductModelGenerator
{
    private static readonly Faker<ProductModel> FakerNoId = Init();

    public static IEnumerable<ProductModel> Generate(int qty, bool withId = true)
    {
        for (var i = 0; i < qty; i++)
        {
            yield return Generate(withId);
        }
    }

    public static ProductModel Generate(bool includeId = true)
    {
        var baseProductModel = FakerNoId.Generate();
        var productModel = new ProductModel
        {
            Company = baseProductModel.Company,
            Description = baseProductModel.Description,
            Name = baseProductModel.Name,
            Tier = baseProductModel.Tier,
            CreatedAt = baseProductModel.CreatedAt,
            ModifiedAt = baseProductModel.ModifiedAt,
            SuggestedPrice = baseProductModel.SuggestedPrice
        };

        if (includeId)
            productModel.Id = Guid.NewGuid();

        return productModel;
    }

    private static Faker<ProductModel> Init()
    {
        var faker = new Faker<ProductModel>()
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Company, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.CreatedAt, f => DateTime.Now.AddDays(f.PickRandom(-90, 0)))
            .RuleFor(p => p.ModifiedAt, f => DateTime.Now.AddDays(f.PickRandom(-90, 0)))
            .RuleFor(p => p.SuggestedPrice, f => decimal.Parse(f.Commerce.Price()))
            .RuleFor(p => p.Tier, f => f.PickRandom<ProductTier>());

        return faker;
    }
    
    public static ProductModel FromAddProductModel(AddProductModel addProductModel, bool addId)
    {
        var productModel = new ProductModel
        {
            Id = addId ? Guid.NewGuid() : Guid.Empty,
            Company = addProductModel.Company,
            Description = addProductModel.Description,
            Name = addProductModel.Name,
            Tier = addProductModel.Tier,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            SuggestedPrice = addProductModel.SuggestedPrice
        };

        return productModel;
    }
}