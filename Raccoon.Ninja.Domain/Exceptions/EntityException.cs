using System.Diagnostics.CodeAnalysis;

namespace Raccoon.Ninja.Domain.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class EntityException: BaseException
{
    public EntityException()
    { }
    
    public EntityException(string message): base(message)
    { }
    
    public EntityException(string message, Exception exception): base(message, exception)
    { }
}