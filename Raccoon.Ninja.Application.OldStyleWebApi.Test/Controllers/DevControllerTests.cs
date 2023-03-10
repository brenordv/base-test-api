using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Raccoon.Ninja.Application.OldStyleWebApi.Controllers;
using Raccoon.Ninja.AppServices.Interfaces;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Test.Controllers;

public class DevControllerTests
{
    private readonly DevController _controller;
    private readonly Mock<IProductsAppService> _productsAppServiceMock;
    private readonly Mock<IUserAppService> _userAppServiceMock;

    public DevControllerTests()
    {
        _productsAppServiceMock = new Mock<IProductsAppService>();
        _userAppServiceMock = new Mock<IUserAppService>();
        _controller = new DevController(_productsAppServiceMock.Object, _userAppServiceMock.Object);
    }

    [Fact]
    public void PopulateUsersDb_ReturnsOk()
    {
        // Arrange
        int? quantity = 10;
        int? archive = 1;

        // Act
        var result = _controller.PopulateUsersDb(quantity, archive);

        // Assert
        result.Should().BeOfType<OkResult>();
        _userAppServiceMock.Verify(x => x.PopulateDevDb(quantity, archive), Times.Once);
    }

    [Fact]
    public void PopulateProductsDb_ReturnsOk()
    {
        // Arrange
        int? quantity = 20;
        int? archive = 0;

        // Act
        var result = _controller.PopulateProductsDb(quantity, archive);

        // Assert
        result.Should().BeOfType<OkResult>();
        _productsAppServiceMock.Verify(x => x.PopulateDevDb(quantity, archive), Times.Once);
    }

    [Fact]
    public void TestRoute_ReturnsOk()
    {
        // Arrange
        // Act
        var result = _controller.TestRoute();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
    }
}