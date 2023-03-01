using Microsoft.AspNetCore.Builder;
using Raccoon.Ninja.Services.Helpers;

namespace Raccoon.Ninja.Infra.DI.Extensions.Web.Common.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication CommonConfig(this WebApplication webApplication)
    {
        // Since all APIs are just for testing, I'll always add the swagger
        webApplication.UseSwagger();
        webApplication.UseSwaggerUI();

        // Adding MethodTimer to log execution time of methods.
        // Since it's a static class, we need to set the logger instance.
        MethodTimeLogger.Logger = webApplication.Logger;

        return webApplication;
    }
}