using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Services.Helpers;

namespace Raccoon.Ninja.Services.Test.Helpers;

public class MethodTimeLoggerTests
{
    [Fact]
    public void Log_WithLoggerSet_ShouldWriteToLogger()
    {
        // Arrange
        var methodBase = MethodBase.GetCurrentMethod();
        var elapsed = TimeSpan.FromSeconds(1);
        const string message = "Test message";
        var logger = new TestLogger();
        MethodTimeLogger.Logger = logger;

        // Act
        MethodTimeLogger.Log(methodBase, elapsed, message);

        // Assert
        logger.LastMessage.Should()
            .Be("MethodTimer: Log_WithLoggerSet_ShouldWriteToLogger took 00:00:01. Message: Test message");
    }

    [Fact]
    public void Log_WithLoggerNotSet_ShouldWriteToConsole()
    {
        // Arrange
        var methodBase = MethodBase.GetCurrentMethod();
        var elapsed = TimeSpan.FromSeconds(1);
        var message = "Test message";
        MethodTimeLogger.Logger = null;

        // Act
        var consoleOutput = ConsoleCapture.Capture(() => MethodTimeLogger.Log(methodBase, elapsed, message));

        // Assert
        consoleOutput.Should()
            .Be("MethodTimer: Log_WithLoggerNotSet_ShouldWriteToConsole took 00:00:01. Message: Test message");
    }
}

internal class TestLogger : ILogger
{
    public string LastMessage { get; private set; }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
        Func<TState, Exception, string> formatter)
    {
        LastMessage = formatter(state, exception);
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable BeginScope<TState>(TState state) => throw new NotImplementedException();
}

internal static class ConsoleCapture
{
    public static string Capture(Action action)
    {
        var originalOutput = Console.Out;
        using var writer = new StringWriter();
        Console.SetOut(writer);

        action();

        Console.SetOut(originalOutput);
        return writer.ToString().TrimEnd();
    }
}