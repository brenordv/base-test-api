using FluentAssertions;
using Moq;
using Raccoon.Ninja.Application.MinimalApi.Endpoints;
using Raccoon.Ninja.AppServices.Interfaces;

namespace Raccoon.Ninja.Application.MinimalApi.Test.Endpoints;

/// <summary>
///     Tests for the Dev endpoints.
/// </summary>
/// <remarks>
///     Pardon the horrible tests. IResults are not really testable in .net6.0.
///     It's planned to be improved (fixed, really) in .net7.0. Whenever I change the .net version, I'll update the tests.
///     Reference: https://github.com/dotnet/aspnetcore/pull/40704
///     (I'm really not happy of the way I'm testing the endpoints.)
/// </remarks>
public class DevEndpointsTests
{
    [Fact]
    public void PopulateProducts_Should_Call_PopulateDevDb_Method_With_Correct_Arguments()
    {
        // Arrange
        var productsAppServiceMock = new Mock<IProductsAppService>();
        int? quantity = 10;
        int? archive = 1;

        // Act
        var result = DevEndpoints.PopulateProducts(productsAppServiceMock.Object, quantity, archive);

        // Assert
        productsAppServiceMock.Verify(x => x.PopulateDevDb(quantity, archive), Times.Once());
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }

    [Fact]
    public void PopulateUsers_Should_Call_PopulateDevDb_Method_With_Correct_Arguments()
    {
        // Arrange
        var usersAppServiceMock = new Mock<IUserAppService>();
        int? quantity = 5;
        int? archive = 0;

        // Act
        var result = DevEndpoints.PopulateUsers(usersAppServiceMock.Object, quantity, archive);

        // Assert
        usersAppServiceMock.Verify(x => x.PopulateDevDb(quantity, archive), Times.Once());
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }

    [Fact]
    public void TestRoute_Ok()
    {
        // Arrange
        // Act
        var result = DevEndpoints.TestRoute();

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }
}