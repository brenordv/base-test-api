using Raccoon.Ninja.Domain.Enums;

namespace Raccoon.Ninja.Domain.Interfaces.Managers;

public interface IEventManager
{
    void Subscribe(EventType eventType, Action listener);
    void Unsubscribe(EventType eventType, Action listener);
    void Notify(EventType eventType);
}