using FluentAssertions;
using Sellers;
using System.Net.Http.Json;
using Xunit;

namespace Orders.api.orders.id;

public class Get_specs
{
    [Fact]
    public async Task Sut_correctly_sets_shop_name()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);

        // Act
        HttpClient client = server.CreateClient();
        string uri = $"api/orders/{orderId}";
        HttpResponseMessage response = await client.GetAsync(uri);
        Order? order = await response.Content.ReadFromJsonAsync<Order>();
        string shopName = order!.ShopName;

        // Assert
        ShopView shop = await server.GetSellersServer().GetShop(order.ShopId);
        shopName.Should().Be(shop.Name);
    }
}
