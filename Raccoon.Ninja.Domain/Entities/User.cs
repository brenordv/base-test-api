using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Validators;

namespace Raccoon.Ninja.Domain.Entities;

public record User
{
    private readonly Guid _id;

    public Guid Id
    {
        get => _id;
        init
        {
            value.EnsureIsValidForId($"{nameof(User)} {nameof(Id)} cannot be empty.");
            _id = value;
        }
    }

    private readonly string _firstName;

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

    private readonly string _lastName;

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

    private readonly string _email;

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

    private readonly string _mobile;

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

    private readonly decimal _credits;

    public decimal Credits
    {
        get => _credits;
        init
        {
            value.IsGreaterThanOrEqualTo(0, $"{nameof(User)} {nameof(Credits)}");
            _credits = value;
        }
    }

    private readonly UserType _role;

    public UserType Role
    {
        get => _role;
        init
        {
            value.IsValidEnum($"{nameof(User)} {nameof(Role)}");
            _role = value;
        }
    }

    private readonly DateTime _createdAt;

    public DateTime CreatedAt
    {
        get => _createdAt;
        init
        {
            value.IsInThePast($"{nameof(User)} {nameof(CreatedAt)}");
            _createdAt = value;
        }
    }

    private readonly DateTime _updatedAt;

    public DateTime UpdatedAt
    {
        get => _updatedAt;
        init
        {
            value.IsInThePast($"{nameof(User)} {nameof(UpdatedAt)}");
            _updatedAt = value;
        }
    }

    private readonly int _version = 1;

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