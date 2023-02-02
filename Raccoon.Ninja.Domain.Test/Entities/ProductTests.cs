using FluentAssertions;
using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Test.TestHelpers;

namespace Raccoon.Ninja.Domain.Test.Entities;

public class ProductTests
{
    [Fact]
    public void Product_InvalidId_ShouldThrowException()
    {
        var action = () => new Product
        {
            Id = Guid.Empty
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product Id cannot be empty.");
    }

    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void Product_InvalidNameNullOrEmpty_ShouldThrowException(string name)
    {
        var action = () => new Product
        {
            Name = name
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product Name cannot be null, empty or empty space.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.Products.NameMaxChars)]
    public void Product_InvalidNameTooBig_ShouldThrowException(string name)
    {
        var action = () => new Product
        {
            Name = name
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"Product Name cannot exceed {EntityConstants.Products.NameMaxChars} characters.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.Products.NameMaxChars)]
    public void Product_ValidName_Success(string name)
    {
        var product = new Product
        {
            Name = name
        };

        product.Name.Should().NotBeNull().And.BeSameAs(name);
    }

    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void Product_InvalidDescriptionNullOrEmpty_ShouldThrowException(string description)
    {
        var action = () => new Product
        {
            Description = description
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product Description cannot be null, empty or empty space.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.Products.DescriptionMaxChars)]
    public void Product_InvalidDescriptionTooBig_ShouldThrowException(string description)
    {
        var action = () => new Product
        {
            Description = description
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage(
                $"Product Description cannot exceed {EntityConstants.Products.DescriptionMaxChars} characters.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.Products.DescriptionMaxChars)]
    public void Product_ValidDescription_Success(string description)
    {
        var product = new Product
        {
            Description = description
        };

        product.Description.Should().NotBeNull().And.BeSameAs(description);
    }

    [Fact]
    public void Product_InvalidSuggestedPrice_ShouldThrowException()
    {
        var action = () => new Product
        {
            SuggestedPrice = -0.00001m
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product SuggestedPrice cannot be lesser than 0.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(0.99)]
    public void Product_ValidSuggestedPrice_Success(decimal suggestedPrice)
    {
        var product = new Product
        {
            SuggestedPrice = suggestedPrice
        };

        product.SuggestedPrice.Should().Be(suggestedPrice);
    }

    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void Product_InvalidCompanyNullOrEmpty_ShouldThrowException(string company)
    {
        var action = () => new Product
        {
            Company = company
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product Company cannot be null, empty or empty space.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.Products.CompanyMaxChars)]
    public void Product_InvalidCompanyTooBig_ShouldThrowException(string company)
    {
        var action = () => new Product
        {
            Company = company
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"Product Company cannot exceed {EntityConstants.Products.CompanyMaxChars} characters.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.Products.CompanyMaxChars)]
    public void Product_ValidCompany_Success(string company)
    {
        var product = new Product
        {
            Company = company
        };

        product.Company.Should().NotBeNull().And.BeSameAs(company);
    }

    [Fact]
    public void Product_InvalidCreatedAtMin_ShouldThrowException()
    {
        var action = () => new Product
        {
            CreatedAt = DateTime.MinValue
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product CreatedAt cannot be equal to minimum date.");
    }

    [Fact]
    public void Product_InvalidCreatedAtMax_ShouldThrowException()
    {
        var action = () => new Product
        {
            CreatedAt = DateTime.MaxValue
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product CreatedAt cannot be equal to maximum date.");
    }

    [Fact]
    public void Product_InvalidCreatedAtFuture_ShouldThrowException()
    {
        var action = () => new Product
        {
            CreatedAt = DateTime.UtcNow.AddDays(1)
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product CreatedAt cannot be in the future.");
    }

    [Fact]
    public void Product_ValidCreatedAt_Success()
    {
        var expectedCreatedAt = DateTime.UtcNow.AddSeconds(-1);


        var product = new Product
        {
            CreatedAt = expectedCreatedAt
        };

        product.CreatedAt.Should().Be(expectedCreatedAt);
    }

    [Fact]
    public void Product_InvalidModifiedAtMin_ShouldThrowException()
    {
        var action = () => new Product
        {
            ModifiedAt = DateTime.MinValue
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product ModifiedAt cannot be equal to minimum date.");
    }

    [Fact]
    public void Product_InvalidModifiedAtMax_ShouldThrowException()
    {
        var action = () => new Product
        {
            ModifiedAt = DateTime.MaxValue
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product ModifiedAt cannot be equal to maximum date.");
    }

    [Fact]
    public void Product_InvalidModifiedAtFuture_ShouldThrowException()
    {
        var action = () => new Product
        {
            ModifiedAt = DateTime.UtcNow.AddDays(1)
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product ModifiedAt cannot be in the future.");
    }

    [Fact]
    public void Product_ValidModifiedAt_Success()
    {
        var expectedModifiedAt = DateTime.UtcNow.AddSeconds(-1);


        var product = new Product
        {
            ModifiedAt = expectedModifiedAt
        };

        product.ModifiedAt.Should().Be(expectedModifiedAt);
    }

    [Fact]
    public void Product_ArchivedAt_NullByDefault()
    {
        var product = new Product();

        product.ArchivedAt.Should().BeNull();
    }

    [Fact]
    public void Product_ArchivedAt_AfterArchiving()
    {
        var now = DateTime.Now;
        var product = new Product();

        product.ArchiveProduct();

        product.ArchivedAt.Should().NotBeNull().And.BeOnOrAfter(now);
        product.IsArchived.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Product_VersionInvalid_ShouldThrowException(int version)
    {
        var action = () => new Product
        {
            Version = version
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product Version cannot be lesser than 1.");
    }

    [Fact]
    public void Product_Version_1ByDefault()
    {
        var product = new Product();

        product.Version.Should().Be(1);
    }

    [Fact]
    public void Product_Version_ChangedOnInit()
    {
        const int version = 42;

        var product = new Product
        {
            Version = version
        };

        product.Version.Should().Be(version);
    }

    [Fact]
    public void Product_Tier_Default()
    {
        var product = new Product();

        product.Tier.Should().Be(ProductTier.F);
    }

    [Fact]
    public void Product_Tier_AfterChange()
    {
        const ProductTier expectedTier = ProductTier.S;
        var product = new Product
        {
            Tier = expectedTier
        };

        product.Tier.Should().Be(expectedTier);
    }

    [Fact]
    public void Product_InvalidTier_ShouldThrowException()
    {
        var action = () => new Product
        {
            Tier = (ProductTier)99999999
        };

        // Assert
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Product Tier '99999999' is not a valid value for ProductTier.");
    }

    #region Test Helpers - Data Generation

    public static IEnumerable<object[]> GetInvalidString()
    {
        return DataGeneration.GetInvalidString();
    }

    public static IEnumerable<object[]> GetValidStringUpToChars(int chars)
    {
        return DataGeneration.GetValidStringUpToChars(chars);
    }

    public static IEnumerable<object[]> GetValidStringOverChars(int chars)
    {
        return DataGeneration.GetValidStringOverChars(chars);
    }

    #endregion
}