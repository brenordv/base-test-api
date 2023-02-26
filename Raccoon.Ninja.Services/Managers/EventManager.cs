using Microsoft.Extensions.Logging;
using Raccoon.Ninja.Domain.Enums;
using Raccoon.Ninja.Domain.Interfaces.Managers;

namespace Raccoon.Ninja.Services.Managers;

public class EventManager : IEventManager
{
    private readonly Dictionary<EventType, HashSet<Action>> _listeners;
    private readonly ILogger<EventManager> _logger;

    public EventManager(ILogger<EventManager> logger)
    {
        _listeners = new Dictionary<EventType, HashSet<Action>>();
        _logger = logger;
    }

    public void Subscribe(EventType eventType, Action listener)
    {
        _logger.LogTrace("Adding subscriber to event: {Event}", eventType);

        if (!_listeners.ContainsKey(eventType))
            _listeners.Add(eventType, new HashSet<Action>());

        _listeners[eventType].Add(listener);
    }

    public void Unsubscribe(EventType eventType, Action listener)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _logger.LogTrace("No subscribers found for event: {Event}", eventType);
            return;
        }

        _logger.LogTrace("Removing subscriber from event: {Event}", eventType);

        _listeners[eventType].Remove(listener);
    }

    public void Notify(EventType eventType)
    {
        if (!_listeners.ContainsKey(eventType))
        {
            _logger.LogTrace("Event '{Event}' is not registered. Nothing to do", eventType);
            return;
        }

        var notifierCount = 0;
        foreach (var subscriberNotifier in _listeners[eventType])
        {
            notifierCount++;

            if (HandleNotification(subscriberNotifier)) continue;

            _logger.LogTrace("Failed to notify subscriber number '{SubscriberNumber}' for event: {Event}",
                notifierCount, eventType);
        }
    }

    private bool HandleNotification(Action callback)
    {
        try
        {
            callback();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogWarning("Notification failed to run. Error: {Error}", e);
            return false;
        }
    }
}