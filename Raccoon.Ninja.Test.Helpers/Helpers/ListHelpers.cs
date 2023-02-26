namespace Raccoon.Ninja.Test.Helpers.Helpers;

public static class ListHelpers
{
    private static readonly Random Random = new();

    public static T RandomPick<T>(this IList<T> list)
    {
        return list[Random.Next(0, list.Count)];
    }
}