using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.Ninja.Domain.Config;

namespace Raccoon.Ninja.Infra.DI.Extensions.Web.Common.Extensions;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddCommon(this ConfigurationManager configurationManager, IServiceCollection services)
    {
        // Adding AppSettings data to environment variables
        configurationManager
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);
        
        // Binding app settings data to Options.
        services.AddOptions<AppSettings>().Bind(configurationManager.GetSection("AppConfig"));
        
        return configurationManager;
    }
}