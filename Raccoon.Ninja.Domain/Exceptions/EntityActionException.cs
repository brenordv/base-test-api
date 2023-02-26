using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Raccoon.Ninja.Domain.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class EntityActionException : BaseException
{
    public EntityActionException()
    {
    }

    public EntityActionException(string message) : base(message)
    {
    }

    public EntityActionException(string message, Exception exception) : base(message, exception)
    {
    }

    protected EntityActionException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}