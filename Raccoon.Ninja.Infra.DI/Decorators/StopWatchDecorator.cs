using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Raccoon.Ninja.Infra.DI.Decorators;

public class StopWatchDecorator<TDecorated> : DispatchProxy
{
    private TDecorated _decorated;
    private ILogger<TDecorated> _logger;

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        var startTime = Stopwatch.GetTimestamp();
        var type = typeof(TDecorated).Name;
        var methodName = $"{type}.{targetMethod.Name}";

        try
        {
            var result = targetMethod.Invoke(_decorated, args);

            return result;
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException ?? ex;
        }
        finally
        {
            //Like stopwatch, but without the overhead of creating a new object + without allocating memory.
            var elapsed = TimeSpan.FromTicks(Stopwatch.GetTimestamp() - startTime);
            _logger.LogTrace("Method {MethodName} took {Elapsed} to execute", methodName, elapsed);
        }
    }

    public static TDecorated Create(TDecorated decorated, ILogger<TDecorated> logger)
    {
        object proxy = Create<TDecorated, StopWatchDecorator<TDecorated>>();
        ((StopWatchDecorator<TDecorated>)proxy).SetParameters(decorated, logger);

        return (TDecorated)proxy;
    }

    private void SetParameters(TDecorated decorated, ILogger<TDecorated> logger)
    {
        _decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        _logger = logger;
    }
}