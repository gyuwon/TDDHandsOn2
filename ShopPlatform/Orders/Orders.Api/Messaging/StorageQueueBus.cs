using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Orders.Events;
using System.Reactive.Disposables;

namespace Orders.Messaging;

internal sealed class StorageQueueBus :
    IBus<PaymentApproved>,
    IAsyncObservable<PaymentApproved>
{
    private readonly QueueClient client;

    public StorageQueueBus(QueueClient client) => this.client = client;

    public Task Send(PaymentApproved message)
        => client.SendMessageAsync(BinaryData.FromObjectAsJson(message));

    public IDisposable Subscribe(Func<PaymentApproved, Task> onNext)
    {
        bool listening = true;
        Process();
        return Disposable.Create(() => listening = false);

        async void Process()
        {
            await client.CreateIfNotExistsAsync();

            while (listening)
            {
                QueueMessage[] messages = await client.ReceiveMessagesAsync();
                foreach (QueueMessage m in messages)
                {
                    if (listening)
                    {
                        await onNext.Invoke(m.Body.ToObjectFromJson<PaymentApproved>());
                        await client.DeleteMessageAsync(m.MessageId, m.PopReceipt);
                    }
                }

                if (messages.Any() == false)
                {
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }
        }
    }
}
