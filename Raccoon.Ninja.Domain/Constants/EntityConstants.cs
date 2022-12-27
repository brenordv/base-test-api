using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Domain.Constants;

[ExcludeFromCodeCoverage]
public static class EntityConstants
{
    public static class Products
    {
        public const int NameMaxChars = 255;
        public const int DescriptionMaxChars = 4000;
        public const int CompanyMaxChars = 512;
    }
}
