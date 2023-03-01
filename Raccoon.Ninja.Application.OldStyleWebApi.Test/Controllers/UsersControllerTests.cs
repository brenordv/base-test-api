using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Raccoon.Ninja.Application.OldStyleWebApi.Controllers;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Test.Controllers;

public class UsersControllerTests
{
    private readonly Mock<IUserAppService> _userAppServiceMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _userAppServiceMock = new Mock<IUserAppService>();
        _controller = new UsersController(_userAppServiceMock.Object);
    }

    [Fact]
    public void Get_ReturnsOk_WithUsers()
    {
        // Arrange
        var users = UserModelGenerator.Generate(2).ToList();
        _userAppServiceMock.Setup(x => x.Get(users.Count)).Returns(users);

        // Act
        var result = _controller.Get(users.Count);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(users);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(null)]
    public void Get_ReturnsNoContent_WithNoUsers(int? limit)
    {
        // Arrange
        var users = new List<UserModel>();
        _userAppServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(users);

        // Act
        var result = _controller.Get(limit);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}