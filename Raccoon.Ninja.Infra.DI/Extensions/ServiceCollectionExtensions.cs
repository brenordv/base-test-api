using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Ninja.Infra.DI.Decorators;

namespace Raccoon.Ninja.Infra.DI.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDecoratedWithMemCache<TService, TImplementation>(
        this IServiceCollection services, ServiceLifetime scope)
        where TService : class
        where TImplementation : class, TService
    {
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), scope));
        services.Decorate<TService, CachingDecorator<TService>>();
        return services;
    }

    public static IServiceCollection AddDecoratedWithStopWatch<TService, TImplementation>(
        this IServiceCollection services, ServiceLifetime scope)
        where TService : class
        where TImplementation : class, TService
    {
        services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), scope));
        services.Decorate<TService, StopWatchDecorator<TService>>();
        return services;
    }

    private static IServiceCollection Decorate<TInterface, TProxy>(this IServiceCollection services)
        where TInterface : class
        where TProxy : DispatchProxy
    {
        MethodInfo createMethod;
        try
        {
            createMethod = typeof(TProxy)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .First(info => !info.IsGenericMethod && info.ReturnType == typeof(TInterface));
        }
        catch (InvalidOperationException e)
        {
            throw new InvalidOperationException(
                $"Looks like there is no static method in {typeof(TProxy)} " +
                $"which creates instance of {typeof(TInterface)} (note that this method should not be generic)",
                e);
        }

        var argInfos = createMethod.GetParameters();

        // Save all descriptors that needs to be decorated into a list.
        var descriptorsToDecorate = services
            .Where(s => s.ServiceType == typeof(TInterface))
            .ToList();

        if (descriptorsToDecorate.Count == 0)
            throw new InvalidOperationException(
                $"Attempted to Decorate services of type {typeof(TInterface)}, " +
                "but no such services are present in ServiceCollection");

        foreach (var descriptor in descriptorsToDecorate)
        {
            var decorated = ServiceDescriptor.Describe(
                typeof(TInterface),
                sp =>
                {
                    var decoratorInstance = createMethod.Invoke(null,
                        argInfos.Select(
                                info => info.ParameterType ==
                                        (descriptor.ServiceType ?? descriptor.ImplementationType)
                                    ? sp.CreateInstance(descriptor)
                                    : sp.GetRequiredService(info.ParameterType))
                            .ToArray());

                    return (TInterface)decoratorInstance;
                },
                descriptor.Lifetime);

            services.Remove(descriptor);
            services.Add(decorated);
        }

        return services;
    }

    private static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
    {
        if (descriptor.ImplementationInstance != null) return descriptor.ImplementationInstance;

        return descriptor.ImplementationFactory != null
            ? descriptor.ImplementationFactory(services)
            : ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType);
    }
}