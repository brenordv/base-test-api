using FluentAssertions;
using Moq;
using Raccoon.Ninja.Domain.Entities;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Exceptions;
using Raccoon.Ninja.Domain.Interfaces.Managers;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Domain.Interfaces.Services;
using Raccoon.Ninja.Services.Services;
using Raccoon.Ninja.Test.Helpers.Generators;

namespace Raccoon.Ninja.Services.Test.Services;

public class ProductServiceTests
{
    private readonly Mock<IEventManager> _eventManagerMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly IProductsService _sut;

    public ProductServiceTests()
    {
        _eventManagerMock = new Mock<IEventManager>();
        _productRepositoryMock = new Mock<IProductRepository>();
        _sut = new ProductsService(_eventManagerMock.Object, _productRepositoryMock.Object);
    }

    [Fact]
    public void Get_ReturnsAllProducts()
    {
        // Arrange
        var expected = ProductGenerator.Generate(5).ToList();

        _productRepositoryMock.Setup(r => r.Get()).Returns(expected);

        // Act
        var result = _sut.Get();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Get_ValidId_ReturnsProduct()
    {
        // Arrange
        var expected = ProductGenerator.Generate(1).Single();

        _productRepositoryMock.Setup(r => r.Get(expected.Id)).Returns(expected);

        // Act
        var result = _sut.Get(expected.Id);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Get_InvalidId_ThrowsException()
    {
        var action = () => _sut.Get(Guid.Empty);

        // Act & Assert
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage(
                "Guid '00000000-0000-0000-0000-000000000000' instance cannot be used as Id. Reasons: Id cannot be empty.");
    }

    [Fact]
    public void Add_AddsNewProduct()
    {
        // Arrange
        var newProduct = ProductGenerator.Generate(false, true);
        var expected = newProduct with { Id = Guid.NewGuid() };
        _productRepositoryMock.Setup(r => r.Add(It.IsAny<Product>())).Returns(expected);

        // Act
        var result = _sut.Add(newProduct);

        // Assert
        result.Should().Be(expected);
        _productRepositoryMock.Verify(r => r.Add(newProduct), Times.Once());
        _eventManagerMock.Verify(e => e.Notify(EventType.ProductsChanged), Times.Once());
    }

    [Fact]
    public void Update_UpdatesExistingProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        const string newProdName = "Updated Product";
        var parsed = new Dictionary<string, object>
        {
            { "Name", newProdName }
        };

        var existingProduct = ProductGenerator.Generate();
        _productRepositoryMock.Setup(r => r.Get(productId)).Returns(existingProduct);
        var expected = existingProduct with { Name = newProdName };
        _productRepositoryMock.Setup(r => r.Update(It.IsAny<Product>())).Returns(expected);

        // Act
        var result = _sut.Update(productId, parsed);

        // Assert
        result.Should().Be(expected);
        _productRepositoryMock.Verify(r => r.Update(It.Is<Product>(p => p.Name == "Updated Product")), Times.Once());
        _eventManagerMock.Verify(e => e.Notify(EventType.ProductsChanged), Times.Once());
    }

    [Fact]
    public void Update_ReturnsNullForNonExistingProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var parsed = new Dictionary<string, object>();
        _productRepositoryMock.Setup(r => r.Get(productId)).Returns((Product)null);

        // Act
        var result = _sut.Update(productId, parsed);

        // Assert
        result.Should().BeNull();
        _productRepositoryMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Never());
        _eventManagerMock.Verify(e => e.Notify(EventType.ProductsChanged), Times.Never());
    }

    [Fact]
    public void PopulateDevDb_WithNullQuantity_ShouldGenerate100Products()
    {
        // Arrange
        const int expectedQuantity = 100;
        const int expectedToArchive = 10;

        _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>()))
            .Returns<Product>(p => p);

        // Act
        _sut.PopulateDevDb(null, expectedToArchive);

        // Assert
        _productRepositoryMock.Verify(repo => repo.Add(It.IsAny<Product>()), Times.Exactly(expectedQuantity));
    }

    [Fact]
    public void PopulateDevDb_WithCustomQuantity_ShouldGenerateCustomNumberOfProducts()
    {
        // Arrange
        const int expectedQuantity = 50;
        const int expectedToArchive = 10;

        _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>()))
            .Returns<Product>(p => p);

        // Act
        _sut.PopulateDevDb(expectedQuantity, expectedToArchive);

        // Assert
        _productRepositoryMock.Verify(repo => repo.Add(It.IsAny<Product>()), Times.Exactly(expectedQuantity));
    }

    [Fact]
    public void PopulateDevDb_WithNullToArchive_ShouldArchive10Products()
    {
        // Arrange
        const int expectedQuantity = 100;
        const int expectedToArchive = 10;

        var insertedProducts = new List<Product>();

        _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>()))
            .Returns<Product>(p => p);

        _productRepositoryMock.Setup(repo => repo.Update(It.IsAny<Product>()))
            .Callback<Product>(p => { insertedProducts.Add(p); });

        // Act
        _sut.PopulateDevDb(expectedQuantity, null);

        // Assert
        _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Exactly(expectedToArchive));
        insertedProducts.Count(p => p.IsArchived).Should().Be(expectedToArchive);
    }

    [Fact]
    public void PopulateDevDb_WithCustomToArchive_ShouldArchive5Products()
    {
        // Arrange
        const int expectedQuantity = 100;
        const int expectedToArchive = 5;

        var insertedProducts = new List<Product>();

        _productRepositoryMock.Setup(repo => repo.Add(It.IsAny<Product>()))
            .Returns<Product>(p => p);

        _productRepositoryMock.Setup(repo => repo.Update(It.IsAny<Product>()))
            .Callback<Product>(p => { insertedProducts.Add(p); });

        // Act
        _sut.PopulateDevDb(expectedQuantity, expectedToArchive);

        // Assert
        _productRepositoryMock.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Exactly(expectedToArchive));
        insertedProducts.Count(p => p.IsArchived).Should().Be(expectedToArchive);
    }
}