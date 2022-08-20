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
}
