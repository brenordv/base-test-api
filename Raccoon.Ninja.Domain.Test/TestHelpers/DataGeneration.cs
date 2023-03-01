using Bogus;

namespace Raccoon.Ninja.Domain.Test.TestHelpers;

public class DataGeneration
{
    public static IEnumerable<object[]> GetInvalidString()
    {
        yield return new object[] { null };
        yield return new object[] { string.Empty };
        yield return new object[] { "       " };
        yield return new object[] { " " };
        yield return new object[] { "" };
    }

    public static IEnumerable<object[]> GetValidStringUpToChars(int chars)
    {
        var faker = new Faker();
        for (var i = 1; i <= chars; i++) yield return new object[] { faker.Random.String(i, i) };
    }

    public static IEnumerable<object[]> GetValidStringOverChars(int chars)
    {
        var faker = new Faker();
        yield return new object[] { faker.Random.String(++chars, chars) };
    }
}