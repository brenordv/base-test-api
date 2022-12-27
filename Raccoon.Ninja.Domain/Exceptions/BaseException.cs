using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Raccoon.Ninja.Domain.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class BaseException: Exception
{
    public BaseException()
    { }
    
    public BaseException(string message): base(message)
    { }
    
    public BaseException(string message, Exception exception): base(message, exception)
    { }
    
    protected BaseException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    { }
}