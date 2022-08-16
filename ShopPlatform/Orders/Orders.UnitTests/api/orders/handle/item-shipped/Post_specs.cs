using FluentAssertions;
using Orders.Events;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Orders.api.orders.handle.item_shipped;

public class Post_specs
{
    [Fact]
    public async Task Sut_correctly_sets_event_time()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.HandleBankTransferPaymentCompleted(orderId);

        DateTime now = DateTime.UtcNow;
        ItemShipped body = new(orderId, EventTimeUtc: now);
        await server.HandleItemShipped(body);

        HttpClient client = server.CreateClient();
        HttpResponseMessage response = await client.GetAsync($"api/orders/{orderId}");
        Order? order = await response.Content.ReadFromJsonAsync<Order>();
        TimeSpan precision = TimeSpan.FromMilliseconds(20);
        order!.ShippedAtUtc.Should().BeCloseTo(now, precision);
    }

    [Fact]
    public async Task Sut_fails_if_order_not_started()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);

        HttpResponseMessage response = await server.HandleItemShipped(orderId);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_fails_if_payment_not_completed()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);

        HttpResponseMessage response = await server.HandleItemShipped(orderId);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_fails_if_order_already_completed()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.HandleBankTransferPaymentCompleted(orderId);
        await server.HandleItemShipped(orderId);

        HttpResponseMessage response = await server.HandleItemShipped(orderId);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
