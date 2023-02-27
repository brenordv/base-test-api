using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Raccoon.Ninja.Application.OldStyleWebApi.Controllers;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Domain.Models;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Application.OldStyleWebApi.Test.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductsAppService> _productsAppServiceMock;

    public ProductsControllerTests()
    {
        _productsAppServiceMock = new Mock<IProductsAppService>();
    }

    [Fact]
    public void Get_ReturnsOkWithListOfProducts_WhenProductsExist()
    {
        // Arrange
        var products = ProductModelGenerator.Generate(3).ToList();
        _productsAppServiceMock.Setup(x => x.Get()).Returns(products);
        var controller = new ProductsController(_productsAppServiceMock.Object);

        // Act
        var result = controller.Get();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(products);
    }

    [Fact]
    public void Get_ReturnsNoContent_WhenNoProductsExist()
    {
        // Arrange
        var products = new List<ProductModel>();
        _productsAppServiceMock.Setup(x => x.Get()).Returns(products);
        var controller = new ProductsController(_productsAppServiceMock.Object);

        // Act
        var result = controller.Get();

        // Assert
        result.Should().BeOfType<NoContentResult>();
        var noContentResult = (NoContentResult)result;
        noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
    }

    [Fact]
    public void GetById_ReturnsOkWithProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = ProductModelGenerator.Generate(1).First();
        product.Id = productId;
        _productsAppServiceMock.Setup(x => x.Get(productId)).Returns(product);
        var controller = new ProductsController(_productsAppServiceMock.Object);

        // Act
        var result = controller.GetById(productId);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = (OkObjectResult)result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value.Should().BeEquivalentTo(product);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productsAppServiceMock.Setup(x => x.Get(productId)).Returns((ProductModel)null);
        var controller = new ProductsController(_productsAppServiceMock.Object);

        // Act
        var result = controller.GetById(productId);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        var notFoundResult = (NotFoundResult)result;
        notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public void AddNewProduct_ReturnsCreatedAtActionWithProduct_WhenProductIsCreated()
    {
        // Arrange
        var addProductModel = AddProductModelGenerator.Generate(1).First();
        var createdProduct = ProductModelGenerator.FromAddProductModel(addProductModel, true);
        _productsAppServiceMock.Setup(x => x.Add(addProductModel)).Returns(createdProduct);
        var controller = new ProductsController(_productsAppServiceMock.Object);

        // Act
        var result = controller.AddNewProduct(addProductModel) as CreatedAtActionResult;

        // Assert
        result.Should().NotBeNull();
        result.ActionName.Should().Be(nameof(ProductsController.GetById));
        result.RouteValues.Should().NotBeNull();
        result.RouteValues.Should().ContainKey("id");
        result.RouteValues["id"].Should().Be(createdProduct.Id);
        result.Value.Should().BeEquivalentTo(createdProduct);
    }
}