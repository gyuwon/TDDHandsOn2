namespace Orders.Messaging;

public interface IBus<T>
{
    Task Send(T message);
}
