using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Interfaces.Managers;
using Raccoon.Ninja.Domain.Interfaces.Services;

namespace Raccoon.Ninja.Services.Services;

public class CacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<CacheService> _logger;
    private readonly IList<object> _keys;

    public CacheService(IMemoryCache memoryCache, IEventManager eventManager, ILogger<CacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        eventManager.Subscribe(EventType.ProductsChanged, ResetCache);
        _keys = new List<object>();
    }

    public bool TryGetValue(object key, out object? value)
    {
        return _memoryCache.TryGetValue(key, out value);
    }

    public void Set(object key, object value)
    {
        _memoryCache.Set(key, value);
        _keys.Add(key);
    }

    private void ResetCache()
    {
        _logger.LogTrace("Busting current cache");
        foreach (var key in _keys)
        {
            _memoryCache.Remove(key);
        }
    }
}