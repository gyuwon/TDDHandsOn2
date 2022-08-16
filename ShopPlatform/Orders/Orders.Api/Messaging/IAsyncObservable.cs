namespace Orders.Messaging;

public interface IAsyncObservable<T>
{
    IDisposable Subscribe(Func<T, Task> onNext);
}
