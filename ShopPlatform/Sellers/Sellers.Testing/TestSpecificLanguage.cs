using Sellers.Commands;
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

    public static Task CreateUser(
        this SellersServer server,
        Guid userId,
        string username,
        string password)
    {
        HttpClient client = server.CreateClient();
        string uri = $"api/users/{userId}/create-user";
        CreateUser body = new(username, password);
        return client.PostAsJsonAsync(uri, body);
    }

    public static Task GrantRole(
        this SellersServer server,
        Guid userId,
        Guid shopId,
        string roleName)
    {
        HttpClient client = server.CreateClient();
        string uri = $"api/users/{userId}/grant-role";
        GrantRole body = new(shopId, roleName);
        return client.PostAsJsonAsync(uri, body);
    }

    public static Task RevokeRole(
        this SellersServer server,
        Guid userId,
        Guid shopId,
        string roleName)
    {
        HttpClient client = server.CreateClient();
        string uri = $"api/users/{userId}/revoke-role";
        RevokeRole body = new(shopId, roleName);
        return client.PostAsJsonAsync(uri, body);
    }

    public static async Task<IEnumerable<Role>> GetRoles(
        this SellersServer server,
        Guid userId)
    {
        HttpClient client = server.CreateClient();
        string uri = $"api/users/{userId}/roles";
        HttpResponseMessage response = await client.GetAsync(uri);
        HttpContent content = response.EnsureSuccessStatusCode().Content;
        return (await content.ReadFromJsonAsync<IEnumerable<Role>>())!;
    }
}
