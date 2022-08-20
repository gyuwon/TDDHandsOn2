using Microsoft.Extensions.DependencyInjection;
using Orders.Commands;
using Orders.Events;
using Sellers;
using System.Net.Http.Json;

namespace Orders;

internal static class TestSpecificLanguage
{
    public static async Task<HttpResponseMessage> PlaceOrder(
        this OrdersServer server,
        Guid orderId)
    {
        string uri = $"api/orders/{orderId}/place-order";

        ShopView shop = await server.GetSellersServer().CreateShop();

        PlaceOrder body = new(
            UserId: Guid.NewGuid(),
            ShopId: shop.Id,
            ItemId: Guid.NewGuid(),
            Price: 100000);

        return await server.CreateClient().PostAsJsonAsync(uri, body);
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

    public static SellersServer GetSellersServer(this OrdersServer server)
        => server.Services.GetRequiredService<SellersServer>();

    public static async Task<ShopView> CreateShop(this SellersServer server)
    {
        HttpClient client = server.CreateClient();
        string uri = "api/shops";
        var body = new { Name = $"{Guid.NewGuid()}" };
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, body);
        return (await response.Content.ReadFromJsonAsync<ShopView>())!;
    }

    public static async Task<ShopView> GetShop(
        this SellersServer server,
        Guid id)
    {
        string uri = $"api/shops/{id}";
        HttpResponseMessage response = await server.CreateClient().GetAsync(uri);
        HttpContent contnet = response.EnsureSuccessStatusCode().Content;
        return (await contnet.ReadFromJsonAsync<ShopView>())!;
    }
}
