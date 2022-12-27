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
    private readonly Mock<IEventManager> _mockEventManager;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly IProductsService _service;

    public ProductServiceTests()
    {
        _mockEventManager = new Mock<IEventManager>();
        _mockProductRepository = new Mock<IProductRepository>();
        _service = new ProductsService(_mockEventManager.Object, _mockProductRepository.Object);
    }

    [Fact]
    public void Get_ReturnsAllProducts()
    {
        // Arrange
        var expected = ProductGenerator.Generate(5).ToList();
        
        _mockProductRepository.Setup(r => r.Get()).Returns(expected);

        // Act
        var result = _service.Get();

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Get_ValidId_ReturnsProduct()
    {
        // Arrange
        var expected = ProductGenerator.Generate(1).Single();
        
        _mockProductRepository.Setup(r => r.Get(expected.Id)).Returns(expected);

        // Act
        var result = _service.Get(expected.Id);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
    
    [Fact]
    public void Get_InvalidId_ThrowsException()
    {
        var action = () => _service.Get(Guid.Empty); 
        
        // Act & Assert
        action
            .Should()
            .Throw<ValidationException>()
            .WithMessage("Guid '00000000-0000-0000-0000-000000000000' instance cannot be used as Id. Reasons: Id cannot be empty.");
    }
    
    [Fact]
    public void Add_AddsNewProduct()
    {
        // Arrange
        var newProduct = ProductGenerator.Generate(false) with { Version = 1};
        var expected = ProductGenerator.Generate(false) with { Version = 1};;
        _mockProductRepository.Setup(r => r.Add(It.IsAny<Product>())).Returns(expected);

        // Act
        var result = _service.Add(newProduct);

        // Assert
        result.Should().Be(expected);
        _mockProductRepository.Verify(r => r.Add(newProduct), Times.Once());
        _mockEventManager.Verify(e => e.Notify(EventType.ProductsChanged), Times.Once());
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
        _mockProductRepository.Setup(r => r.Get(productId)).Returns(existingProduct);
        var expected = existingProduct with { Name = newProdName };
        _mockProductRepository.Setup(r => r.Update(It.IsAny<Product>())).Returns(expected); 

        // Act
        var result = _service.Update(productId, parsed);

        // Assert
        result.Should().Be(expected);
        _mockProductRepository.Verify(r => r.Update(It.Is<Product>(p => p.Name == "Updated Product")), Times.Once());
        _mockEventManager.Verify(e => e.Notify(EventType.ProductsChanged), Times.Once());
    }

    [Fact]
    public void Update_ReturnsNullForNonExistingProduct()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var parsed = new Dictionary<string, object>();
        _mockProductRepository.Setup(r => r.Get(productId)).Returns((Product)null);

        // Act
        var result = _service.Update(productId, parsed);

        // Assert
        result.Should().BeNull();
        _mockProductRepository.Verify(r => r.Update(It.IsAny<Product>()), Times.Never());
        _mockEventManager.Verify(e => e.Notify(EventType.ProductsChanged), Times.Never());
    }
}