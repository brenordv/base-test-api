using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Interfaces.Managers;
using Raccoon.Ninja.Services.Managers;

namespace Raccoon.Ninja.Services.Test.Managers;

public class EventManagerTests
{
    private readonly Mock<ILogger<EventManager>> _loggerMock;
    private readonly EventManager _sut;

    public EventManagerTests()
    {
        _loggerMock = new Mock<ILogger<EventManager>>();
        _sut = new EventManager(_loggerMock.Object);
    }

    [Fact]
    public void Notify_NoOneToNotify()
    {
        // Act
        _sut.Notify(EventType.ProductsChanged);

        // Assert
        _sut.Should().NotBeNull();
        _sut.Should().BeOfType<EventManager>();
        _sut.GetType().Should().Implement<IEventManager>();
        
        _loggerMock.Verify(logger => logger.Log(
            LogLevel.Trace, 
            It.IsAny<EventId>(), 
            It.Is<It.IsAnyType>((o, t) => string.Equals("Event '{Event}' is not registered. Nothing to do", o.ToString(), StringComparison.InvariantCultureIgnoreCase)), 
            null, 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ),  Times.Never);
    }
    
    
    [Fact]
    public void Notify_NotifiesAllListenersForEvent()
    {
        // Arrange
        const EventType eventType = EventType.ProductsChanged;
        var listener1Called = false;
        var listener2Called = false;

        var listener1 = new Action(() => { listener1Called = true; });
        var listener2 = new Action(() => { listener2Called = true; });

        _sut.Subscribe(eventType, listener1);
        _sut.Subscribe(eventType, listener2);
        _sut.Unsubscribe(eventType, listener2);
        
        // Act
        _sut.Notify(eventType);

        // Assert
        _sut.Should().NotBeNull();
        _sut.Should().BeOfType<EventManager>();
        _sut.GetType().Should().Implement<IEventManager>();

        listener1Called.Should().BeTrue();
        listener2Called.Should().BeFalse();

        _loggerMock.Verify(logger => logger.Log(
            LogLevel.Trace, 
            It.IsAny<EventId>(), 
            It.Is<It.IsAnyType>((o, t) => string.Equals("Failed to notify subscriber number '{SubscriberNumber}' for event: {Event}", o.ToString(), StringComparison.InvariantCultureIgnoreCase)), 
            null, 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ),  Times.Never);
        
        _loggerMock.Verify(logger => logger.Log(
            LogLevel.Trace, 
            It.IsAny<EventId>(), 
            It.Is<It.IsAnyType>((o, t) => string.Equals("Event '{Event}' is not registered. Nothing to do", o.ToString(), StringComparison.InvariantCultureIgnoreCase)), 
            null, 
            It.IsAny<Func<It.IsAnyType, Exception, string>>()
        ),  Times.Never);
    }
}