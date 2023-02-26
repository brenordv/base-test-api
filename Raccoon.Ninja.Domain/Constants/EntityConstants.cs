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

    public static class User
    {
        public const int FirstNameMaxChars = 255;
        public const int LastNameMaxChars = 255;
        public const int EmailMaxChars = 512;
        public const int MobileMaxChars = 50;
    }
}