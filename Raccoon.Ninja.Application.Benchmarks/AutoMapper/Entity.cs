namespace Raccoon.Ninja.Application.Benchmarks.AutoMapper;

public record Entity()
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public float BaseValue { get; init; }
    public float TaxPercent { get; init; }
    public DateTime CreatedAt { get; init; }
}