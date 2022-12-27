using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Repositories.DbHelpers;

/// <summary>
/// Either returns the database file that will be used or
/// the memory stream.
/// <remarks>
/// This will be improved in the future. 
/// </remarks>
/// </summary>
[ExcludeFromCodeCoverage]
public static class DbFileHelper
{
    public static string FromFile(string folder)
    {
        const string dbFileName = "apiData.dev.db";
        var dbFile = string.IsNullOrWhiteSpace(folder)
            ? $".\\{dbFileName}"
            : Path.Combine(folder, dbFileName); 
        return $"Filename={dbFile};Connection=shared";
    }

    public static MemoryStream FromMemory()
    {
        return new MemoryStream();
    }
}