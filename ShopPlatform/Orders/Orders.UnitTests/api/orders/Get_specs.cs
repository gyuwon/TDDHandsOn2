using FluentAssertions;
using Orders.Commands;
using Sellers;
using System.Net.Http.Json;
using Xunit;

namespace Orders.api.orders;

public class Get_specs
{
    [Fact]
    public async Task Sut_correctly_filters_orders_by_user_id()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        ShopView shop = await server.GetSellersServer().CreateShop();
        Guid itemId = Guid.NewGuid();
        decimal price = 100000;

        List<PlaceOrder> commands = new()
        {
            new PlaceOrder(UserId: Guid.NewGuid(), shop.Id, itemId, price),
            new PlaceOrder(UserId: Guid.NewGuid(), shop.Id, itemId, price),
            new PlaceOrder(UserId: Guid.NewGuid(), shop.Id, itemId, price),
        };

        HttpClient client = server.CreateClient();

        await Task.WhenAll(from command in commands
                           let orderId = Guid.NewGuid()
                           let uri = $"api/orders/{orderId}/place-order"
                           orderby orderId
                           select client.PostAsJsonAsync(uri, command));

        Guid userId = commands[0].UserId;

        // Act
        string queryUri = $"api/orders?user-id={userId}";
        HttpResponseMessage response = await client.GetAsync(queryUri);

        // Assert
        Order[]? orders = await response.Content.ReadFromJsonAsync<Order[]>();
        orders.Should().OnlyContain(x => x.UserId == userId);
    }

    [Fact]
    public async Task Sut_correctly_filters_orders_by_shop_id()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        ShopView shop1 = await server.GetSellersServer().CreateShop();
        ShopView shop2 = await server.GetSellersServer().CreateShop();
        ShopView shop3 = await server.GetSellersServer().CreateShop();

        Guid userId = Guid.NewGuid();
        Guid itemId = Guid.NewGuid();
        decimal price = 100000;

        List<PlaceOrder> commands = new()
        {
            new PlaceOrder(userId, shop1.Id, itemId, price),
            new PlaceOrder(userId, shop2.Id, itemId, price),
            new PlaceOrder(userId, shop3.Id, itemId, price),
        };

        HttpClient client = server.CreateClient();

        await Task.WhenAll(from command in commands
                           let orderId = Guid.NewGuid()
                           let uri = $"api/orders/{orderId}/place-order"
                           orderby orderId
                           select client.PostAsJsonAsync(uri, command));

        // Act
        string queryUri = $"api/orders?shop-id={shop1.Id}";
        HttpResponseMessage response = await client.GetAsync(queryUri);

        // Assert
        Order[]? orders = await response.Content.ReadFromJsonAsync<Order[]>();
        orders.Should().OnlyContain(x => x.ShopId == shop1.Id);
    }

    [Fact]
    public async Task Sut_correctly_filters_orders_by_user_id_and_shop_id()
    {
        // Arrange
        OrdersServer server = OrdersServer.Create();

        ShopView shop1 = await server.GetSellersServer().CreateShop();
        ShopView shop2 = await server.GetSellersServer().CreateShop();

        Guid userId = Guid.NewGuid();
        Guid itemId = Guid.NewGuid();
        decimal price = 100000;

        HttpClient client = server.CreateClient();

        List<PlaceOrder> commands = new()
        {
            new PlaceOrder(userId, shop2.Id, itemId, price),
            new PlaceOrder(userId, shop1.Id, itemId, price),
            new PlaceOrder(UserId: Guid.NewGuid(), shop1.Id, itemId, price),
        };

        await Task.WhenAll(from command in commands
                           let orderId = Guid.NewGuid()
                           let uri = $"api/orders/{orderId}/place-order"
                           orderby orderId
                           select client.PostAsJsonAsync(uri, command));

        // Act
        string queryUri = $"api/orders?user-id={userId}&shop-id={shop1.Id}";
        HttpResponseMessage response = await client.GetAsync(queryUri);

        // Assert
        Order[]? orders = await response.Content.ReadFromJsonAsync<Order[]>();
        orders.Should().OnlyContain(x => x.UserId == userId && x.ShopId == shop1.Id);
    }
}
