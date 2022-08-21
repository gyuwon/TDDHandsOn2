using System.Net.Http.Json;

namespace Sellers;

public static class TestSpecificLanguage
{
    public static async Task<ShopView> CreateShop(this SellersServer server)
    {
        HttpClient client = server.CreateClient();
        string uri = "api/shops";
        var body = new { Name = $"{Guid.NewGuid()}" };
        HttpResponseMessage response = await client.PostAsJsonAsync(uri, body);
        return (await response.Content.ReadFromJsonAsync<ShopView>())!;
    }

    public static Task<HttpResponseMessage> SetShopUser(
        this SellersServer server,
        Guid shopId,
        string username,
        string password)
    {
        HttpClient client = server.CreateClient();
        string uri = $"api/shops/{shopId}/user";
        ShopUser body = new(username, password);
        return client.PostAsJsonAsync(uri, body);
    }
}
