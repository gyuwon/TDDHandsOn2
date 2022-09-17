using System.Runtime.Serialization;

namespace Sellers.CommandModel;

public class EntityNotFoundException : InvalidOperationException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string? message)
        : base(message)
    {
    }

    public EntityNotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected EntityNotFoundException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
}
