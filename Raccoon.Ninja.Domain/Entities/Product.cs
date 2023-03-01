using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Extensions;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Entities;

public record Product
{
    private readonly string _company;

    private readonly DateTime _createdAt;

    private readonly string _description;

    private readonly Guid _id;

    private readonly DateTime _modifiedAt;

    private readonly string _name;

    private readonly decimal _suggestedPrice;

    private readonly ProductTier _tier;

    private readonly int _version = 1;

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

    public Guid Id
    {
        get => _id;
        init
        {
            value.EnsureIsValidForId($"{nameof(Product)} {nameof(Id)} cannot be empty.");
            _id = value;
        }
    }

    public string Name
    {
        get => _name;
        init
        {
            value.IsTextUpToChars(EntityConstants.Products.NameMaxChars, $"{nameof(Product)} {nameof(Name)}");

            _name = value;
        }
    }

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

    public decimal SuggestedPrice
    {
        get => _suggestedPrice;
        init
        {
            value.IsGreaterThanOrEqualTo(0, $"{nameof(Product)} {nameof(SuggestedPrice)}");
            _suggestedPrice = value;
        }
    }

    public ProductTier Tier
    {
        get => _tier;
        init
        {
            value.IsValidEnum($"{nameof(Product)} {nameof(Tier)}");
            _tier = value;
        }
    }

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

    public DateTime CreatedAt
    {
        get => _createdAt;
        init
        {
            value.IsInThePast($"{nameof(Product)} {nameof(CreatedAt)}");
            _createdAt = value;
        }
    }

    public DateTime ModifiedAt
    {
        get => _modifiedAt;
        init
        {
            value.IsInThePast($"{nameof(Product)} {nameof(ModifiedAt)}");
            _modifiedAt = value;
        }
    }

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