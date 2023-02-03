using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Raccoon.Ninja.Services.Helpers;

/// <summary>
/// This class must be named like this to be recognized by the MethodTimer NuGet package.
/// </summary>
public static class MethodTimeLogger
{
    public static ILogger Logger;   
    public static void Log(MethodBase methodBse, TimeSpan elapsed, string message)
    {
        //https://learn.microsoft.com/en-us/azure/azure-monitor/app/tutorial-asp-net-custom-metrics#aggregation
        //https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics#trackmetric
        
        //Idea: Send custom telemetry data to Azure Application Insights, so we can monitor performance degradation.
        
        if (Logger != null)
        {
            Logger.LogTrace("MethodTimer: {method} took {elapsed}. Message: {message}",
                methodBse.Name, elapsed, message);
            return;
        }
        
        Console.WriteLine($"MethodTimer: {methodBse.Name} took {elapsed}. Message: {message}");
    }
}