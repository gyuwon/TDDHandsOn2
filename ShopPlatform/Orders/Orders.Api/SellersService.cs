namespace Orders;

public sealed class SellersService
{
    private readonly HttpClient client;

    public SellersService(HttpClient client) => this.client = client;

    public async Task<Shop?> FindShop(Guid id)
    {
        HttpResponseMessage response = await client.GetAsync($"api/shops/{id}");
        return response.IsSuccessStatusCode switch
        {
            true => await response.Content.ReadFromJsonAsync<Shop>(),
            _ => null,
        };
    }

    public async Task<Shop> GetShop(Guid id)
    {
        HttpResponseMessage response = await client.GetAsync($"api/shops/{id}");
        HttpContent content = response.EnsureSuccessStatusCode().Content;
        return (await content.ReadFromJsonAsync<Shop>())!;
    }
}
