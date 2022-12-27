using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Exceptions;

namespace Raccoon.Ninja.Domain.Entities;

public record Product
{
    public Product()
    {
    }

    public Product(Product old, IDictionary<string, object> newValues)
    {
        Id = old.Id;
        Name = newValues.TryGetValue(nameof(Name), out var name) ? name.ToString() : old.Name;
        Company = newValues.TryGetValue(nameof(Company), out var company) ? company.ToString() : old.Company;
        Description = newValues.TryGetValue(nameof(Description), out var description)
            ? description.ToString()
            : old.Description;
        SuggestedPrice = newValues.TryGetValue(nameof(SuggestedPrice), out var suggestedPrice)
            ? decimal.Parse(suggestedPrice.ToString())
            : old.SuggestedPrice;
        
        Tier = newValues.TryGetValue(nameof(Tier), out var tier) ? (ProductTier)tier : old.Tier;
        CreatedAt = newValues.TryGetValue(nameof(CreatedAt), out var createdAt) ? (DateTime)createdAt : old.CreatedAt;
        ModifiedAt = newValues.TryGetValue(nameof(ModifiedAt), out var modifiedAt) ? (DateTime)modifiedAt : old.ModifiedAt;
        ArchivedAt = newValues.TryGetValue(nameof(ArchivedAt), out var archivedAt) ? (DateTime)archivedAt : old.ArchivedAt;
        Version = old.Version + 1;
    }

    private readonly Guid _id;

    public Guid Id
    {
        get => _id;
        init
        {
            if (value == Guid.Empty)
                throw new EntityException($"{nameof(Product)} {nameof(Id)} cannot be empty.");

            _id = value;
        }
    }

    private readonly string _name;

    public string Name
    {
        get => _name;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityException($"{nameof(Product)} {nameof(Name)} cannot be null, empty or empty space.");

            if (value.Length > EntityConstants.Products.NameMaxChars)
                throw new EntityException(
                    $"{nameof(Product)} {nameof(Name)} cannot exceed {EntityConstants.Products.NameMaxChars} characters.");

            _name = value;
        }
    }

    private readonly string _description;

    public string Description
    {
        get => _description;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityException(
                    $"{nameof(Product)} {nameof(Description)} cannot be null, empty or empty space.");

            if (value.Length > EntityConstants.Products.DescriptionMaxChars)
                throw new EntityException(
                    $"{nameof(Product)} {nameof(Description)} cannot exceed {EntityConstants.Products.DescriptionMaxChars} characters.");

            _description = value;
        }
    }

    private readonly decimal _suggestedPrice;

    public decimal SuggestedPrice
    {
        get => _suggestedPrice;
        init
        {
            if (value < 0)
                throw new EntityException($"{nameof(Product)} {nameof(SuggestedPrice)} cannot be lesser than zero.");

            _suggestedPrice = value;
        }
    }

    private readonly ProductTier _tier;

    public ProductTier Tier
    {
        get => _tier;
        init
        {
            if (!Enum.IsDefined(value))
                throw new EntityException($"{nameof(Product)} {nameof(Tier)} value must be defined.");

            _tier = value;
        }
    }

    private readonly string _company;

    public string Company
    {
        get => _company;
        init
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityException($"{nameof(Product)} {nameof(Company)} cannot be null, empty or empty space.");

            if (value.Length > EntityConstants.Products.CompanyMaxChars)
                throw new EntityException(
                    $"{nameof(Product)} {nameof(Company)} cannot exceed {EntityConstants.Products.CompanyMaxChars} characters.");

            _company = value;
        }
    }

    private readonly DateTime _createdAt;

    public DateTime CreatedAt
    {
        get => _createdAt;
        init
        {
            if (value == DateTime.MinValue)
                throw new EntityException($"{nameof(Product)} {nameof(CreatedAt)} cannot be equal to minimum date.");

            if (value == DateTime.MaxValue)
                throw new EntityException($"{nameof(Product)} {nameof(CreatedAt)} cannot be equal to maximum date.");

            if (value.ToUniversalTime() > DateTime.UtcNow)
                throw new EntityException($"{nameof(Product)} {nameof(CreatedAt)} cannot be in the future.");

            _createdAt = value;
        }
    }

    private readonly DateTime _modifiedAt;

    public DateTime ModifiedAt
    {
        get => _modifiedAt;
        init
        {
            if (value == DateTime.MinValue)
                throw new EntityException($"{nameof(Product)} {nameof(ModifiedAt)} cannot be equal to minimum date.");

            if (value == DateTime.MaxValue)
                throw new EntityException($"{nameof(Product)} {nameof(ModifiedAt)} cannot be equal to maximum date.");

            if (value.ToUniversalTime() > DateTime.UtcNow)
                throw new EntityException($"{nameof(Product)} {nameof(ModifiedAt)} cannot be in the future.");

            _modifiedAt = value;
        }
    }

    private readonly int _version = 1;

    public int Version
    {
        get => _version;
        init
        {
            if (value <= 0)
                throw new EntityException($"{nameof(Product)} {nameof(Version)} cannot be lesser than zero.");

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