using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;

namespace Sellers.api.shops;

public class Get_specs
{
    private const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=Sellers_80d18902;User Id=postgres;Password=mysecretpassword;";

    [Theory, AutoSellersData]
    public async Task Sut_returns_all_shops(
        [ConnectionString(ConnectionString)] SellersServer server,
        Shop[] shops)
    {
        IServiceProvider services = server.Services.CreateScope().ServiceProvider;
        SellersDbContext context = services.GetRequiredService<SellersDbContext>();
        context.RemoveRange(await context.Shops.ToListAsync());
        context.AddRange(shops);
        await context.SaveChangesAsync();

        HttpResponseMessage response = await server.CreateClient().GetAsync("api/shops");
        Shop[]? actual = await response.Content.ReadFromJsonAsync<Shop[]>();

        actual.Should().BeEquivalentTo(shops, c => c.Excluding(x => x.Sequence));
    }
}
