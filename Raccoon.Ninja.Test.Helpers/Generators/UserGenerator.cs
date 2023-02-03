using Bogus;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Test.Helpers.Generators;

public static class UserGenerator
{
    private static readonly Faker<User> _fakerFull = Init(true);
    private static readonly Faker<User> _fakerNoId = Init(false);

    public static IEnumerable<User> Generate(int qty, bool withId = true)
    {
        for (var i = 0; i < qty; i++)
        {
            yield return Generate(withId);
        }   
    }
    
    private static User Generate(bool includeId = true, bool resetVersion = false)
    {
        return includeId ? _fakerFull.Generate() : _fakerNoId.Generate();
    }
    
    private static Faker<User> Init(bool includeId)
    {
        var faker = new Faker<User>()
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Mobile, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.Credits, f => f.Random.Decimal())
            .RuleFor(p => p.Role, f => f.Random.Enum<UserType>())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past())
            .RuleFor(p => p.UpdatedAt, f => f.Date.Past())
            .RuleFor(p => p.Version, f => f.PickRandom(1, 42));

        if (includeId)
            faker.RuleFor(p => p.Id, f => Guid.NewGuid());

        return faker;
    }
}