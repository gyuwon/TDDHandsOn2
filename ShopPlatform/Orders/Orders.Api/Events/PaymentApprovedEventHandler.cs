using Microsoft.EntityFrameworkCore;
using Orders.Messaging;

namespace Orders.Events;

internal static class PaymentApprovedEventHandler
{
    public static void Listen(IServiceProvider provider)
    {
        IAsyncObservable<PaymentApproved> stream =
            provider.GetRequiredService<IAsyncObservable<PaymentApproved>>();

        stream.Subscribe(async (PaymentApproved listenedEvent) =>
        {
            using IServiceScope scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

            IQueryable<Order> query =
                from x in context.Orders
                where x.PaymentTransactionId == listenedEvent.TransactionId
                select x;

            if (await query.SingleOrDefaultAsync() is Order order)
            {
                order.Status = OrderStatus.AwaitingShipment;
                order.PaidAtUtc = listenedEvent.EventTimeUtc;
                await context.SaveChangesAsync();
            }
        });
    }
}
