using Polly;

namespace Orders;

public static class DefaultPolicy
{
    private static readonly Random random = new();

    public static IAsyncPolicy Instance { get; } = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(retryCount: 5, GetDelay);

    private static TimeSpan GetDelay(int retryAttempt)
    {
        int delay = 100;
        for (int i = 1; i < retryAttempt; i++)
        {
            delay = delay * 2 + random.Next(20);
        }

        return TimeSpan.FromMilliseconds(delay);
    }
}
