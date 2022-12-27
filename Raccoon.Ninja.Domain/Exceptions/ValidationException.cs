using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Raccoon.Ninja.Domain.Exceptions;

[ExcludeFromCodeCoverage]
[Serializable]
public class ValidationException: BaseException
{
    public ValidationException()
    { }
    
    public ValidationException(string message): base(message)
    { }
    
    public ValidationException(string message, Exception exception): base(message, exception)
    { }
    
    protected ValidationException(SerializationInfo info, StreamingContext context) 
        : base(info, context)
    { }
}