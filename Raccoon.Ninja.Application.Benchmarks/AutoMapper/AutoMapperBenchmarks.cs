using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Bogus;

namespace Raccoon.Ninja.Application.Benchmarks.AutoMapper;

[ExcludeFromCodeCoverage]
[MemoryDiagnoser, RankColumn, MinColumn, MaxColumn, MeanColumn, MedianColumn,UnicodeConsoleLogger, Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class AutoMapperBenchmarks
{
    
    #region Benchmark Setup
    private IMapper _mapper;
    private Entity[] _entities;

    [Params(1, 10, 100, 1000)]
    public int QuantityEntities { get; set; }

    private void CreateMapper()
    {
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Entity, Model>()
                .ForMember(
                    dest => dest.FinalValue, 
                    opt => 
                        opt.MapFrom(src => src.BaseValue + src.BaseValue * src.TaxPercent));
            
        }).CreateMapper();
    }
    
    private void CreateEntities()
    {
        var faker = new Faker<Entity>()
            .RuleFor(x => x.Id, f => f.Random.Guid())
            .RuleFor(x => x.Name, f => f.Commerce.ProductName())
            .RuleFor(x => x.CreatedAt, f => f.Date.Past())
            .RuleFor(x => x.Description, f => f.Commerce.ProductDescription())
            .RuleFor(x => x.BaseValue, f => f.Random.Float(0.5f, 4200f))
            .RuleFor(x => x.TaxPercent, f => f.Random.Float());

        _entities = faker.Generate(QuantityEntities).ToArray();
    }
    
    [GlobalSetup]
    public void Init()
    {
        CreateMapper();
        CreateEntities();
    }
    
    #endregion

    [Benchmark]
    public void AutoMapper_SingleEntityConverted()
    {
        foreach (var entity in _entities)
        {
            var model = _mapper.Map<Model>(entity);
        }
    }
    
    [Benchmark]
    public void AutoMapper_ListOfEntitiesConverted()
    {
        var models = _mapper.Map<Model[]>(_entities);
    }
    
    [Benchmark]
    public void Explicit_SingleEntityConverted()
    {
        foreach (var entity in _entities)
        {
            var model = (Model)entity;
        }
    }
    
    [Benchmark]
    public void Implicit_SingleEntityConverted()
    {
        foreach (var entity in _entities)
        {
            Model model = entity;
        }
    }
    
    [Benchmark]
    public void Explicit_ListOfEntitiesLINQConverted()
    {
        var models = _entities.Select(x => (Model)x).ToArray();
    }
        
    [Benchmark]
    public void Implicit_ListOfEntitiesLINQConverted()
    {
        var models = _entities.Select(x =>
        {
            Model model = x;
            return model;
        }).ToArray();
    }
    
    [Benchmark]
    public void DirectAssignment_SingleEntityConverted()
    {
        foreach (var entity in _entities)
        {
            var model = new Model
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                FinalValue = entity.BaseValue + entity.BaseValue * entity.TaxPercent
            };
        }
    }
    
    [Benchmark]
    public void DirectAssignment_ListOfEntitiesConverted()
    {
        var models = _entities.Select(entity => new Model
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            FinalValue = entity.BaseValue + entity.BaseValue * entity.TaxPercent
        }).ToArray();
    }
}