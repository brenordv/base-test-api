using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Domain.Models;

[ExcludeFromCodeCoverage]
[Serializable]
public class UpdateProductModel
{
    [JsonProperty("id")] public Guid Id { get; set; }
    [JsonProperty("name")] public string Name { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("suggestedPrice")] public decimal? SuggestedPrice { get; set; }
    [JsonProperty("tier")] public ProductTier? Tier { get; set; }
    [JsonProperty("company")] public string Company { get; set; }
    [JsonProperty("createdAt")] public DateTime? CreatedAt { get; set; }
    [JsonProperty("modifiedAt")] public DateTime? ModifiedAt { get; set; }

    [JsonProperty("archivedAt", NullValueHandling = NullValueHandling.Ignore)]
    public DateTime? ArchivedAt { get; set; }
}