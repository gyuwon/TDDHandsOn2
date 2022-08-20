using FluentAssertions;
using Orders.Commands;
using Sellers;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Orders.api.orders.id.place_order;

public class Post_specs
{
    [Fact]
    public async Task Sut_returns_BadRequest_if_shop_not_exists()
    {
        OrdersServer server = OrdersServer.Create();
        string uri = $"api/orders/{Guid.NewGuid()}/place-order";
        PlaceOrder body = new(
            UserId: Guid.NewGuid(),
            ShopId: Guid.NewGuid(),
            ItemId: Guid.NewGuid(),
            Price: 100000);

        HttpClient client = server.CreateClient();
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, body);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_returns_OK_if_shop_exists()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        ShopView shop = await server.GetSellersServer().CreateShop();

        string uri = $"api/orders/{Guid.NewGuid()}/place-order";

        PlaceOrder body = new(
            UserId: Guid.NewGuid(),
            ShopId: shop.Id,
            ItemId: Guid.NewGuid(),
            Price: 100000);

        // Act
        HttpClient client = server.CreateClient();
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, body);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Sut_does_not_create_order_if_shop_not_exists()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        Guid orderId = Guid.NewGuid();

        string commandUri = $"api/orders/{orderId}/place-order";

        PlaceOrder body = new(
            UserId: Guid.NewGuid(),
            ShopId: Guid.NewGuid(),
            ItemId: Guid.NewGuid(),
            Price: 100000);

        HttpClient client = server.CreateClient();

        // Act
        await client.PostAsJsonAsync(commandUri, body);

        // Assert
        string queryUri = $"api/orders/{orderId}";
        HttpResponseMessage response = await client.GetAsync(queryUri);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
