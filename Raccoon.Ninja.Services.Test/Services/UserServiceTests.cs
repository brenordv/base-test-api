using FluentAssertions;
using Moq;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Services.Services;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Services.Test.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public void Get_ReturnsExpectedResult()
    {
        // Arrange
        const int quantity = 5;
        var expectedUsers = UserGenerator.Generate(quantity).ToList();
        _userRepositoryMock.Setup(r => r.Get()).Returns(expectedUsers);

        // Act
        var result = _userService.Get(2);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedUsers.Take(2));
    }

    [Fact]
    public void PopulateDevDb_InsertsExpectedNumberOfUsers()
    {
        // Arrange
        const int expectedInserted = 200;
        const int expectedDeactivated = 20;

        var expectedUser = UserGenerator.Generate(1).First();
        _userRepositoryMock.Setup(r => r.Add(It.IsAny<User>())).Returns(expectedUser);

        // Act
        _userService.PopulateDevDb(expectedInserted, expectedDeactivated);

        // Assert
        _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Exactly(expectedInserted));
        _userRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Exactly(expectedDeactivated));
    }
}