#nullable enable
namespace Raccoon.Ninja.Domain.Interfaces.Services;

public interface ICacheService
{
    bool TryGetValue(object key, out object? value);
    void Set(object key, object value);
}