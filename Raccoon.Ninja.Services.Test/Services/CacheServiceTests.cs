using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Raccoon.Ninja.Domain.Interfaces.Managers;
using Raccoon.Ninja.Services.Services;

namespace Raccoon.Ninja.Services.Test.Services;

public class CacheServiceTests
{
    private readonly CacheService _cacheService;
    private readonly Mock<IMemoryCache> _mockMemoryCache;

    public CacheServiceTests()
    {
        var eventManagerMock = new Mock<IEventManager>();
        var loggerMock = new Mock<ILogger<CacheService>>();
        _mockMemoryCache = new Mock<IMemoryCache>();
        _cacheService = new CacheService(_mockMemoryCache.Object, eventManagerMock.Object, loggerMock.Object);
    }

    [Fact]
    public void TryGetValue_ExistingKey_ReturnsTrue()
    {
        // Arrange
        _mockMemoryCache
            .Setup(cache => cache.TryGetValue("key", out It.Ref<object>.IsAny))
            .Returns(true);

        // Act
        var result = _cacheService.TryGetValue("key", out var value);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void TryGetValue_ExistingKey_ReturnsValue()
    {
        // Arrange
        const string expectedValue = "value";
        _mockMemoryCache
            .Setup(cache => cache.TryGetValue("key", out It.Ref<object>.IsAny))
            .Callback((object key, out object v) => { v = expectedValue; })
            .Returns(true);

        // Act
        var result = _cacheService.TryGetValue("key", out var value);

        // Assert
        result.Should().BeTrue();
        value.Should().Be(expectedValue);
    }

    [Fact]
    public void TryGetValue_NonExistingKey_ReturnsFalse()
    {
        // Arrange
        _mockMemoryCache
            .Setup(cache => cache.TryGetValue("key", out It.Ref<object>.IsAny))
            .Returns(false);

        // Act
        var result = _cacheService.TryGetValue("key", out var value);

        // Assert
        result.Should().BeFalse();
        value.Should().BeNull();
    }

    [Fact]
    public void TryGetValue_NullKey_ReturnsFalse()
    {
        // Arrange


        // Act
        var result = _cacheService.TryGetValue(null, out var value);

        // Assert
        result.Should().BeFalse();
    }
}