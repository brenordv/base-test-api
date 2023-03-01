using Microsoft.Extensions.DependencyInjection;
using Raccoon.Ninja.AppServices.AppServices;
using Raccoon.Ninja.AppServices.Helpers;
using Raccoon.Ninja.AppServices.Interfaces;
using Raccoon.Ninja.Infra.DI.Helpers;

namespace Raccoon.Ninja.Infra.DI.Extensions.Web.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterCommon(this IServiceCollection services)
    {
        // Basic API configuration.
        services
            .AddEndpointsApiExplorer()
            .AddMemoryCache()
            .AddLogging();
        
        // Since all APIs are just for testing, I'll always add the swagger
        services.AddSwaggerGen();
        
        // Registering API adapter (app service layer) specific stuff.
        services
            .RegisterMappingProfiles()
            .AddDecoratedWithStopWatch<IProductsAppService, ProductsAppService>(ServiceLifetime.Scoped)
            .AddScoped<IUserAppService, UsersAppService>();

        // Registering the actual Services, Repositories and auxiliary engines.
        services
            .RegisterRepositories()
            .RegisterServices()
            .RegisterAuxiliary();
        
        return services;
    }
}