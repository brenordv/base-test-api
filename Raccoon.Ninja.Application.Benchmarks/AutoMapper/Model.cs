using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Application.Benchmarks.AutoMapper;

[ExcludeFromCodeCoverage]
public record Model
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public float FinalValue { get; init; }
    
    public static implicit operator Model(Entity entity)
    {
        return new Model
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            FinalValue = entity.BaseValue + entity.BaseValue * entity.TaxPercent
        };
    }
}