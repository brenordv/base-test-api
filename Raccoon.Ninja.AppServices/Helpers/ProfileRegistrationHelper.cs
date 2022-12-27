using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Ninja.AppServices.MapperProfiles;

namespace Raccoon.Ninja.AppServices.Helpers;

public static class ProfileRegistrationHelper
{
    public static IServiceCollection RegisterMappingProfiles(this IServiceCollection services)
    {
        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new ApiProfile());
        });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}