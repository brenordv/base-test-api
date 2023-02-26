using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Extensions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Entities;

public record Product
{
    public Product()
    {
    }

    public Product(Product old, IDictionary<string, object> newValues)
    {
        // Properties not allowed to be updated by the user.
        Id = old.Id;
        CreatedAt = old.CreatedAt;

        // Dynamic properties. (Can be updated by the user)
        Name = newValues.TryGetValue(nameof(Name), out var name) ? name.ToString() : old.Name;
        Company = newValues.TryGetValue(nameof(Company), out var company) ? company.ToString() : old.Company;
        Description = newValues.TryGetValue(nameof(Description), out var description)
            ? description.ToString()
            : old.Description;

        if (newValues.TryGetValue(nameof(SuggestedPrice), out var suggestedPrice))
            SuggestedPrice = suggestedPrice.TryParseDecimal(old.SuggestedPrice);

        Tier = newValues.TryGetValue(nameof(Tier), out var tier) ? (ProductTier)tier : old.Tier;
        ArchivedAt = newValues.TryGetValue(nameof(ArchivedAt), out var archivedAt)
            ? (DateTime)archivedAt
            : old.ArchivedAt;
    }

    private readonly Guid _id;

    public Guid Id
    {
        get => _id;
        init
        {
            value.EnsureIsValidForId($"{nameof(Product)} {nameof(Id)} cannot be empty.");
            _id = value;
        }
    }

    private readonly string _name;

    public string Name
    {
        get => _name;
        init
        {
            value.IsTextUpToChars(EntityConstants.Products.NameMaxChars, $"{nameof(Product)} {nameof(Name)}");

            _name = value;
        }
    }

    private readonly string _description;

    public string Description
    {
        get => _description;
        init
        {
            value.IsTextUpToChars(EntityConstants.Products.DescriptionMaxChars,
                $"{nameof(Product)} {nameof(Description)}");

            _description = value;
        }
    }

    private readonly decimal _suggestedPrice;

    public decimal SuggestedPrice
    {
        get => _suggestedPrice;
        init
        {
            value.IsGreaterThanOrEqualTo(0, $"{nameof(Product)} {nameof(SuggestedPrice)}");
            _suggestedPrice = value;
        }
    }

    private readonly ProductTier _tier;

    public ProductTier Tier
    {
        get => _tier;
        init
        {
            value.IsValidEnum($"{nameof(Product)} {nameof(Tier)}");
            _tier = value;
        }
    }

    private readonly string _company;

    public string Company
    {
        get => _company;
        init
        {
            value.IsTextUpToChars(EntityConstants.Products.CompanyMaxChars,
                $"{nameof(Product)} {nameof(Company)}");

            _company = value;
        }
    }

    private readonly DateTime _createdAt;

    public DateTime CreatedAt
    {
        get => _createdAt;
        init
        {
            value.IsInThePast($"{nameof(Product)} {nameof(CreatedAt)}");
            _createdAt = value;
        }
    }

    private readonly DateTime _modifiedAt;

    public DateTime ModifiedAt
    {
        get => _modifiedAt;
        init
        {
            value.IsInThePast($"{nameof(Product)} {nameof(ModifiedAt)}");
            _modifiedAt = value;
        }
    }

    private readonly int _version = 1;

    public int Version
    {
        get => _version;
        init
        {
            value.IsGreaterThanOrEqualTo(_version, $"{nameof(Product)} {nameof(Version)}");
            _version = value;
        }
    }

    public DateTime? ArchivedAt { get; private set; }

    public bool IsArchived => ArchivedAt.HasValue;

    public void ArchiveProduct()
    {
        ArchivedAt = DateTime.Now;
    }
}