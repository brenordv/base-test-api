using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Domain.Models;

[ExcludeFromCodeCoverage]
[Serializable]
public class AddProductModel
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("suggestedPrice")] public decimal SuggestedPrice { get; set; }
    [JsonProperty("tier")] public ProductTier Tier { get; set; }
    [JsonProperty("Company")] public string Company { get; set; }
}