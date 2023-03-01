using Bogus;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Test.Helpers.Generators;

public static class UserModelGenerator
{
    private static readonly Faker<UserModel> FakerFull = Init(true);
    private static readonly Faker<UserModel> FakerNoId = Init(false);

    public static IEnumerable<UserModel> Generate(int qty, bool withId = true)
    {
        for (var i = 0; i < qty; i++) yield return Generate(withId);
    }

    private static UserModel Generate(bool includeId = true)
    {
        return includeId ? FakerFull.Generate() : FakerNoId.Generate();
    }

    private static Faker<UserModel> Init(bool includeId)
    {
        var faker = new Faker<UserModel>()
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.Mobile, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.Role, f => f.Random.Enum<UserType>())
            .RuleFor(p => p.CreatedAt, f => f.Date.Past())
            .RuleFor(p => p.UpdatedAt, f => f.Date.Past())
            .RuleFor(p => p.IsActive, f => f.Random.Bool());

        if (includeId)
            faker.RuleFor(p => p.Id, f => Guid.NewGuid());

        return faker;
    }
}