using Microsoft.AspNetCore.TestHost;
using Orders.Commands;
using Orders.Events;
using System.Net.Http.Json;

namespace Orders;

internal static class TestSpecificLanguage
{
    public static Task<HttpResponseMessage> PlaceOrder(
        this OrdersServer server,
        Guid orderId)
    {
        string uri = $"api/orders/{orderId}/place-order";

        PlaceOrder body = new(
            UserId: Guid.NewGuid(),
            ShopId: Guid.NewGuid(),
            ItemId: Guid.NewGuid(),
            Price: 100000);

        return server.CreateClient().PostAsJsonAsync(uri, body);
    }

    public static Task<HttpResponseMessage> StartOrder(
        this OrdersServer server,
        Guid orderId,
        string? paymentTransactionId = null)
    {
        string uri = $"api/orders/{orderId}/start-order";
        StartOrder body = new(paymentTransactionId);
        return server.CreateClient().PostAsJsonAsync(uri, body);
    }

    public static Task<HttpResponseMessage> HandleBankTransferPaymentCompleted(
        this OrdersServer server,
        Guid orderId)
    {
        string uri = "api/orders/handle/bank-transder-payment-completed";
        DateTime now = DateTime.UtcNow;
        BankTransferPaymentCompleted body = new(orderId, EventTimeUtc: now);
        return server.CreateClient().PostAsJsonAsync(uri, body);
    }

    public static Task<HttpResponseMessage> HandleItemShipped(
        this OrdersServer server,
        Guid orderId)
    {
        ItemShipped body = new(orderId, EventTimeUtc: DateTime.UtcNow);
        return HandleItemShipped(server, body);
    }

    public static Task<HttpResponseMessage> HandleItemShipped(
        this OrdersServer server,
        ItemShipped body)
    {
        string uri = "api/orders/handle/item-shipped";
        return server.CreateClient().PostAsJsonAsync(uri, body);
    }
}
