using FluentAssertions;
using Moq;
using Raccoon.Ninja.Application.MinimalApi.Endpoints;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Application.MinimalApi.Test.Endpoints;

/// <summary>
///     Tests for the Product endpoints.
/// </summary>
/// <remarks>
///     Pardon the horrible tests. IResults are not really testable in .net6.0.
///     It's planned to be improved (fixed, really) in .net7.0. Whenever I change the .net version, I'll update the tests.
///     Reference: https://github.com/dotnet/aspnetcore/pull/40704
///     (I'm really not happy of the way I'm testing the endpoints.)
/// </remarks>
public class ProductEndpointsTests
{
    private readonly ProductModel _nullProductModel = null;

    [Fact]
    public void GetProducts_Should_Return_OkResult_With_Products_When_Products_Exist()
    {
        // Arrange
        var productModels = ProductGenerator.Generate(10).Select(product => (ProductModel)product).ToList();
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Get()).Returns(productModels);

        // Act
        var result = ProductEndpoints.GetProducts(productsAppServiceMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }

    [Fact]
    public void GetProducts_Should_Return_NoContentResult_When_Products_Do_Not_Exist()
    {
        // Arrange
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Get()).Returns(new List<ProductModel>());

        // Act
        var result = ProductEndpoints.GetProducts(productsAppServiceMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("NoContentResult");
    }

    [Fact]
    public void GetProductById_Should_Return_OkResult_With_Product_When_Product_Exists()
    {
        // Arrange
        ProductModel product = ProductGenerator.Generate();
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Get(product.Id)).Returns(product);

        // Act
        var result = ProductEndpoints.GetProductById(productsAppServiceMock.Object, product.Id);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }

    [Fact]
    public void GetProductById_Should_Return_NotFoundResult_When_Product_Does_Not_Exist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Get(productId)).Returns(_nullProductModel);

        // Act
        var result = ProductEndpoints.GetProductById(productsAppServiceMock.Object, productId);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("NotFoundObjectResult");
    }

    [Fact]
    public void CreateProduct_Should_Return_CreatedResult_With_Product_When_Creation_Succeeds()
    {
        // Arrange
        var addProductModel = new AddProductModel { Name = "Product1" };
        var createdProduct = new ProductModel { Id = Guid.NewGuid(), Name = addProductModel.Name };
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Add(addProductModel)).Returns(createdProduct);

        // Act
        var result = ProductEndpoints.CreateProduct(productsAppServiceMock.Object, addProductModel);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("CreatedResult");
    }

    [Fact]
    public void CreateProduct_Should_Return_BadRequestResult_When_Creation_Fails()
    {
        // Arrange
        var addProductModel = new AddProductModel { Name = "Product1" };
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Add(addProductModel)).Returns(_nullProductModel);

        // Act
        var result = ProductEndpoints.CreateProduct(productsAppServiceMock.Object, addProductModel);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("BadRequestObjectResult");
    }

    [Fact]
    public void UpdateProduct_Should_Return_OkResult_With_Updated_Product_When_Update_Succeeds()
    {
        // Arrange
        var updateProductModel = new UpdateProductModel { Id = Guid.NewGuid(), Name = "Product1" };
        var updatedProduct = new ProductModel { Id = updateProductModel.Id, Name = updateProductModel.Name };
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Update(updateProductModel)).Returns(updatedProduct);

        // Act
        var result = ProductEndpoints.UpdateProduct(productsAppServiceMock.Object, updateProductModel);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("OkObjectResult");
    }

    [Fact]
    public void UpdateProduct_Should_Return_NotFoundResult_When_Product_Does_Not_Exist()
    {
        // Arrange
        var updateProductModel = new UpdateProductModel { Id = Guid.NewGuid(), Name = "Product1" };
        var productsAppServiceMock = new Mock<IProductsAppService>();
        productsAppServiceMock.Setup(x => x.Update(updateProductModel)).Returns(_nullProductModel);

        // Act
        var result = ProductEndpoints.UpdateProduct(productsAppServiceMock.Object, updateProductModel);

        // Assert
        result.Should().NotBeNull();
        result.ToString().Should().EndWith("NotFoundObjectResult");
    }
}