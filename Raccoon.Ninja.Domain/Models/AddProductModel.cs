using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Domain.Models;

[ExcludeFromCodeCoverage]
[Serializable]
public class AddProductModel
{
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("suggestedPrice")] public decimal SuggestedPrice { get; set; }
    [JsonProperty("tier")] public ProductTier Tier { get; set; }
    [JsonProperty("company")] public string Company { get; set; }

    public static implicit operator Product(AddProductModel model)
    {
        return new Product
        {
            Name = model.Name,
            Description = model.Description,
            SuggestedPrice = model.SuggestedPrice,
            Tier = model.Tier,
            Company = model.Company
        };
    }

    public static implicit operator AddProductModel(Product product)
    {
        return new AddProductModel
        {
            Name = product.Name,
            Description = product.Description,
            SuggestedPrice = product.SuggestedPrice,
            Tier = product.Tier,
            Company = product.Company
        };
    }
}