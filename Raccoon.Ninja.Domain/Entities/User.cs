using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Entities;

public record User
{
    private readonly DateTime _createdAt;

    private readonly decimal _credits;

    private readonly string _email;

    private readonly string _firstName;
    private readonly Guid _id;

    private readonly string _lastName;

    private readonly string _mobile;

    private readonly UserType _role;

    private readonly DateTime _updatedAt;

    private readonly int _version = 1;

    public Guid Id
    {
        get => _id;
        init
        {
            value.EnsureIsValidForId($"{nameof(User)} {nameof(Id)} cannot be empty.");
            _id = value;
        }
    }

    public string FirstName
    {
        get => _firstName;
        init
        {
            value.IsTextUpToChars(EntityConstants.User.FirstNameMaxChars,
                $"{nameof(User)} {nameof(FirstName)}");
            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        init
        {
            value.IsTextUpToChars(EntityConstants.User.LastNameMaxChars,
                $"{nameof(User)} {nameof(LastName)}");
            _lastName = value;
        }
    }

    public string Email
    {
        get => _email;
        init
        {
            value.IsTextUpToChars(EntityConstants.User.EmailMaxChars,
                $"{nameof(User)} {nameof(Email)}");
            _email = value;
        }
    }

    public string Mobile
    {
        get => _mobile;
        init
        {
            value.IsTextUpToChars(EntityConstants.User.MobileMaxChars,
                $"{nameof(User)} {nameof(Mobile)}");
            _mobile = value;
        }
    }

    public decimal Credits
    {
        get => _credits;
        init
        {
            value.IsGreaterThanOrEqualTo(0, $"{nameof(User)} {nameof(Credits)}");
            _credits = value;
        }
    }

    public UserType Role
    {
        get => _role;
        init
        {
            value.IsValidEnum($"{nameof(User)} {nameof(Role)}");
            _role = value;
        }
    }

    public DateTime CreatedAt
    {
        get => _createdAt;
        init
        {
            value.IsInThePast($"{nameof(User)} {nameof(CreatedAt)}");
            _createdAt = value;
        }
    }

    public DateTime UpdatedAt
    {
        get => _updatedAt;
        init
        {
            value.IsInThePast($"{nameof(User)} {nameof(UpdatedAt)}");
            _updatedAt = value;
        }
    }

    public int Version
    {
        get => _version;
        init
        {
            value.IsGreaterThanOrEqualTo(_version, $"{nameof(User)} {nameof(Version)}");
            _version = value;
        }
    }

    public bool IsActive { get; init; }
}