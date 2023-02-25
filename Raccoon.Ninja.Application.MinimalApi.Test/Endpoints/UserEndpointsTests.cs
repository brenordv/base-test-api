using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Raccoon.Ninja.Application.MinimalApi.Endpoints;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;

namespace Raccoon.Ninja.Application.MinimalApi.Test.Endpoints;

public class UserEndpointsTests
{
    [Fact]
    public void GetUsers_ReturnsOkObjectResult_WhenUsersExist()
    {
        // Arrange
        var userAppServiceMock = new Mock<IUserAppService>();
        var users = new[] { new UserModel { Id = Guid.NewGuid(), FirstName = "Alice" }, new UserModel { Id = Guid.NewGuid(), FirstName = "Bob" } };
        userAppServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(users);

        // Act
        var result = UserEndpoints.GetUsers(userAppServiceMock.Object, null);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }

    [Fact]
    public void GetUsers_ReturnsNoContentResult_WhenUsersDoNotExist()
    {
        // Arrange
        var userAppServiceMock = new Mock<IUserAppService>();
        userAppServiceMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new List<UserModel>());

        // Act
        var result = UserEndpoints.GetUsers(userAppServiceMock.Object, null);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("NoContentResult");
    }
}