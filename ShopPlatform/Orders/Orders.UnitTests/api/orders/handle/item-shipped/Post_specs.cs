using FluentAssertions;
using Orders.Commands;
using Orders.Events;
using System.Net.Http.Json;
using Xunit;

namespace Orders.api.orders.handle.item_shipped;

public class Post_specs
{
    [Fact]
    public async Task Sut_correctly_sets_event_time()
    {
        // Arrange
        HttpClient client = OrdersServer.Create().CreateClient();

        Guid orderId = Guid.NewGuid();

        PlaceOrder placeOrder = new(
            UserId: Guid.NewGuid(),
            ShopId: Guid.NewGuid(),
            ItemId: Guid.NewGuid(),
            Price: 100000);
        await client.PostAsJsonAsync($"api/orders/{orderId}/place-order", placeOrder);

        StartOrder startOrder = new();
        await client.PostAsJsonAsync($"api/orders/{orderId}/start-order", startOrder);

        BankTransferPaymentCompleted paymentCompleted = new(
            orderId,
            EventTimeUtc: DateTime.UtcNow);
        await client.PostAsJsonAsync("api/orders/handle/bank-transder-payment-completed", paymentCompleted);

        // Act
        DateTime now = DateTime.UtcNow;
        ItemShipped itemShipped = new(orderId, EventTimeUtc: now);
        await client.PostAsJsonAsync("api/orders/handle/item-shipped", itemShipped);

        // Assert
        HttpResponseMessage response = await client.GetAsync($"api/orders/{orderId}");
        Order? order = await response.Content.ReadFromJsonAsync<Order>();
        TimeSpan precision = TimeSpan.FromMilliseconds(20);
        order!.ShippedAtUtc.Should().BeCloseTo(now, precision);
    }
}
