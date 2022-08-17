using FluentAssertions;
using Orders.Events;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Orders.api.orders.accept.payment_approved;

public class Post_specs
{
    [Fact]
    public async Task Sut_correctly_changes_order_status()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        Guid orderId = Guid.NewGuid();
        string paymentTransactionId = $"{Guid.NewGuid()}";

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId, paymentTransactionId);

        HttpClient client = server.CreateClient();

        string uri = "api/orders/accept/payment-approved";

        ExternalPaymentApproved body = new(
            tid: paymentTransactionId,
            approved_at: DateTime.UtcNow);

        // Act
        await client.PostAsJsonAsync(uri, body);

        // Assert
        await DefaultPolicy.Instance.ExecuteAsync(async () => {
            HttpResponseMessage response = await client.GetAsync($"api/orders/{orderId}");
            Order? order = await response.Content.ReadFromJsonAsync<Order>();
            order!.Status.Should().Be(OrderStatus.AwaitingShipment);
        });
    }

    [Fact]
    public async Task Sut_returns_Accepted()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        Guid orderId = Guid.NewGuid();
        string paymentTransactionId = $"{Guid.NewGuid()}";

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId, paymentTransactionId);


        string uri = "api/orders/accept/payment-approved";

        ExternalPaymentApproved body = new(
            tid: paymentTransactionId,
            approved_at: DateTime.UtcNow);

        // Act
        HttpResponseMessage response = await server.CreateClient().PostAsJsonAsync(uri, body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    [Fact]
    public async Task Sut_correctly_sets_event_time()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        Guid orderId = Guid.NewGuid();
        string paymentTransactionId = $"{Guid.NewGuid()}";

        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId, paymentTransactionId);

        HttpClient client = server.CreateClient();

        string uri = "api/orders/accept/payment-approved";

        ExternalPaymentApproved body = new(
            tid: paymentTransactionId,
            approved_at: DateTime.UtcNow);

        // Act
        await client.PostAsJsonAsync(uri, body);

        // Assert
        await DefaultPolicy.Instance.ExecuteAsync(async () => {
            HttpResponseMessage response = await client.GetAsync($"api/orders/{orderId}");
            Order? order = await response.Content.ReadFromJsonAsync<Order>();
            TimeSpan precision = TimeSpan.FromMilliseconds(20);
            order!.PaidAtUtc.Should().BeCloseTo(body.approved_at, precision);
        });
    }
}
