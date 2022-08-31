using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Sellers.api.users.id.roles;

public class Get_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_NotFound_with_nonexistent_id(
        SellersServer server,
        Guid id)
    {
        string uri = $"api/users/{id}/roles";
        HttpResponseMessage actual = await server.CreateClient().GetAsync(uri);
        actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_OK_with_existing_id(
        SellersServer server,
        Shop shop)
    {
        using IServiceScope scope = server.Services.CreateScope();
        SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        string uri = $"api/users/{shop.Id}/roles";
        HttpResponseMessage actual = await server.CreateClient().GetAsync(uri);

        actual.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoSellersData]
    public async Task Sut_correctly_returns_roles(
        SellersServer server,
        Shop shop)
    {
        using IServiceScope scope = server.Services.CreateScope();
        SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        string uri = $"api/users/{shop.Id}/roles";
        HttpResponseMessage response = await server.CreateClient().GetAsync(uri);

        Role[]? actual = await response.Content.ReadFromJsonAsync<Role[]>();
        actual.Should().BeEquivalentTo(new[] { new Role(shop.Id, "Administrator") });
    }
}
