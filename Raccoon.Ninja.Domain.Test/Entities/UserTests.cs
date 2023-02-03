using FluentAssertions;
using Raccoon.Ninja.Domain.Constants;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Test.TestHelpers;

namespace Raccoon.Ninja.Domain.Test.Entities;

public class UserTests
{
    [Fact]
    public void User_InvalidId_ShouldThrowException()
    {
        var action = () => new User
        {
            Id = Guid.Empty
        };

        action.Should().Throw<ValidationException>().WithMessage("User Id cannot be empty.");
    }

    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void User_InvalidFirstNameNullOrEmpty_ShouldThrowException(string firstName)
    {
        var action = () => new User
        {
            FirstName = firstName
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User FirstName cannot be null, empty or empty space.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.User.FirstNameMaxChars)]
    public void User_InvalidFirstNameTooBig_ShouldThrowException(string firstName)
    {
        var action = () => new User
        {
            FirstName = firstName
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"User FirstName cannot exceed {EntityConstants.User.FirstNameMaxChars} characters.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.User.FirstNameMaxChars)]
    public void User_ValidFirstName_Success(string firstName)
    {
        var user = new User
        {
            FirstName = firstName
        };

        user.FirstName.Should().NotBeNull().And.BeSameAs(firstName);
    }

    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void User_InvalidLastNameNullOrEmpty_ShouldThrowException(string lastName)
    {
        var action = () => new User
        {
            LastName = lastName
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User LastName cannot be null, empty or empty space.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.User.LastNameMaxChars)]
    public void User_InvalidLastNameTooBig_ShouldThrowException(string lastName)
    {
        var action = () => new User
        {
            LastName = lastName
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"User LastName cannot exceed {EntityConstants.User.LastNameMaxChars} characters.");
    }

    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.User.LastNameMaxChars)]
    public void User_ValidLastName_Success(string lastName)
    {
        var user = new User
        {
            LastName = lastName
        };

        user.LastName.Should().NotBeNull();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void User_InvalidEmailNullOrEmpty_ShouldThrowException(string email)
    {
        var action = () => new User
        {
            Email = email
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User Email cannot be null, empty or empty space.");
    }
    
    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.User.EmailMaxChars)]
    public void User_InvalidEmailTooBig_ShouldThrowException(string email)
    {
        var action = () => new User
        {
            Email = email
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"User Email cannot exceed {EntityConstants.User.EmailMaxChars} characters.");
    }
    
    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.User.EmailMaxChars)]
    public void User_ValidEmail_Success(string email)
    {
        var user = new User
        {
            Email = email
        };

        user.Email.Should().NotBeNull();
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidString))]
    public void User_InvalidMobileNullOrEmpty_ShouldThrowException(string mobile)
    {
        var action = () => new User
        {
            Mobile = mobile
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User Mobile cannot be null, empty or empty space.");
    }
    
    [Theory]
    [MemberData(nameof(GetValidStringOverChars), EntityConstants.User.MobileMaxChars)]
    public void User_InvalidMobileTooBig_ShouldThrowException(string mobile)
    {
        var action = () => new User
        {
            Mobile = mobile
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage($"User Mobile cannot exceed {EntityConstants.User.MobileMaxChars} characters.");
    }
    
    [Theory]
    [MemberData(nameof(GetValidStringUpToChars), EntityConstants.User.MobileMaxChars)]
    public void User_ValidMobile_Success(string mobile)
    {
        var user = new User
        {
            Mobile = mobile
        };

        user.Mobile.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-2)]
    public void Credits_InvalidValue_ShouldThrowException(decimal credits)
    {
        var action = () => new User
        {
            Credits = credits
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User Credits cannot be lesser than 0.");
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void Credits_ValidValue_Success(decimal credits)
    {
        var user = new User
        {
            Credits = credits
        };

        user.Credits.Should().Be(credits);
    }
    
    [Fact]
    public void Role_InvalidValue_ShouldThrowException()
    {
        var action = () => new User
        {
            Role = (UserType) 100
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User Role '100' is not a valid value for UserType.");
    }
    
    [Theory]
    [InlineData(UserType.Admin)]
    [InlineData(UserType.Free)]
    public void Role_ValidValue_Success(UserType role)
    {
        var user = new User
        {
            Role = role
        };

        user.Role.Should().Be(role);
    }
    
    [Fact]
    public void CreatedAt_InvalidValueInTheFuture_ShouldThrowException()
    {
        var action = () => new User
        {
            CreatedAt = DateTime.UtcNow.AddDays(1)
        };

        action.Should().Throw<ValidationException>().WithMessage("User CreatedAt cannot be in the future.");
    }
    
    [Fact]
    public void CreatedAt_InvalidValueMin_ShouldThrowException()
    {
        var action = () => new User
        {
            CreatedAt = DateTime.MinValue
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User CreatedAt cannot be equal to minimum date.");
    }

    [Fact]
    public void CreatedAt_InvalidValueMax_ShouldThrowException()
    {
        var action = () => new User
        {
            CreatedAt = DateTime.MaxValue
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User CreatedAt cannot be equal to maximum date.");
    }
    
    [Fact]
    public void CreatedAt_ValidValue_Success()
    {
        var user = new User
        {
            CreatedAt = DateTime.UtcNow
        };

        user.CreatedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(500));
    }
    
    
    
    [Fact]
    public void UpdatedAt_InvalidValue_ShouldThrowException()
    {
        var action = () => new User
        {
            UpdatedAt = DateTime.UtcNow.AddDays(1)
        };

        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("User UpdatedAt cannot be in the future.");
    }
    
    [Fact]
    public void UpdatedAt_ValidValue_Success()
    {
        var user = new User
        {
            UpdatedAt = DateTime.UtcNow
        };

        user.UpdatedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromMilliseconds(500));
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