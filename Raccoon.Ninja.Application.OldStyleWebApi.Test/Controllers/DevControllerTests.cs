using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Raccoon.Ninja.Application.OldStyleWebApi.Controllers;
using Raccoon.Ninja.AppServices.Interfaces;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Test.Controllers;

public class DevControllerTests
{
    private readonly Mock<IProductsAppService> _productsAppServiceMock;
    private readonly Mock<IUserAppService> _userAppServiceMock;
    private readonly DevController _controller;

    public DevControllerTests()
    {
        _productsAppServiceMock = new Mock<IProductsAppService>();
        _userAppServiceMock = new Mock<IUserAppService>();
        _controller = new DevController(_productsAppServiceMock.Object, _userAppServiceMock.Object);
    }

    [Fact]
    public async Task PopulateUsersDb_ReturnsOk()
    {
        // Arrange
        int? quantity = 10;
        int? archive = 1;

        // Act
        var result = await _controller.PopulateUsersDb(quantity, archive);

        // Assert
        result.Should().BeOfType<OkResult>();
        _userAppServiceMock.Verify(x => x.PopulateDevDb(quantity, archive), Times.Once);
    }

    [Fact]
    public async Task PopulateProductsDb_ReturnsOk()
    {
        // Arrange
        int? quantity = 20;
        int? archive = 0;

        // Act
        var result = await _controller.PopulateProductsDb(quantity, archive);

        // Assert
        result.Should().BeOfType<OkResult>();
        _productsAppServiceMock.Verify(x => x.PopulateDevDb(quantity, archive), Times.Once);
    }
}