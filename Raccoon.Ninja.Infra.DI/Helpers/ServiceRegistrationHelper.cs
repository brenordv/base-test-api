using Microsoft.Extensions.DependencyInjection;
using Raccoon.Ninja.Domain.Interfaces.Managers;
using Raccoon.Ninja.Domain.Interfaces.Repositories;
using Raccoon.Ninja.Domain.Interfaces.Services;
using Raccoon.Ninja.Infra.DI.Extensions;
using Raccoon.Ninja.Repositories.Repositories;
using Raccoon.Ninja.Services.Managers;
using Raccoon.Ninja.Services.Services;

namespace Raccoon.Ninja.Infra.DI.Helpers;

public static class ServiceRegistrationHelper
{
    public static IServiceCollection RegisterAuxiliary(this IServiceCollection services)
    {
        services.AddSingleton<IEventManager, EventManager>();
        return services;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }

    public static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddDecoratedWithMemCache<IProductRepository, ProductRepository>(ServiceLifetime.Singleton);
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}