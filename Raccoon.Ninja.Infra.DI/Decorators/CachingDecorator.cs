using System.Reflection;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Interfaces.Services;

namespace Raccoon.Ninja.Infra.DI.Decorators;

public class CachingDecorator<TDecorated> : DispatchProxy
{
    private TDecorated _decorated;
    private ICacheService _cacheService;
    private ILogger<TDecorated> _logger;

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var type = typeof(TDecorated).Name;
        var key = $"{type}.{targetMethod.Name}.{string.Join("--", args)}";
        try
        {
            if (_cacheService.TryGetValue(key, out var cachedResult))
            {
                _logger.LogTrace("Method {MethodName} returned a cached value", targetMethod.Name);
                return cachedResult;
            }

            var result = targetMethod.Invoke(_decorated, args);
            _cacheService.Set(key, result);

            return result;
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException ?? ex;
        }
    }

    public static TDecorated Create(TDecorated decorated, ICacheService cacheService, ILogger<TDecorated> logger)
    {
        object proxy = Create<TDecorated, CachingDecorator<TDecorated>>();
        ((CachingDecorator<TDecorated>)proxy).SetParameters(decorated, cacheService, logger);

        return (TDecorated)proxy;
    }

    private void SetParameters(TDecorated decorated, ICacheService cacheService, ILogger<TDecorated> logger)
    {
        _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        _cacheService = cacheService;
        _logger = logger;
    }
}