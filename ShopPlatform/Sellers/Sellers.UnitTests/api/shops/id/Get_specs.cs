using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;
using Xunit;

namespace Sellers.api.shops.id;

public class Get_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_does_not_expose_user_credentials(
        SellersServer server,
        Shop shop)
    {
        // Arrange
        IServiceScope scope = server.Services.CreateScope();
        SellersDbContext context = scope.ServiceProvider.GetRequiredService<SellersDbContext>();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        // Act
        HttpClient client = server.CreateClient();
        HttpResponseMessage response = await client.GetAsync($"api/shops/{shop.Id}");
        Shop? actual = await response.Content.ReadFromJsonAsync<Shop>();

        // Assert
        actual!.UserId.Should().BeNull();
        actual.PasswordHash.Should().BeNull();
    }
}
