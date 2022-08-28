using AutoFixture.Xunit2;
using FluentAssertions;
using System.Collections.Immutable;
using Xunit;

namespace Sellers.QueryModel;

public class ShopUserReader_specs
{
    [Fact]
    public void Sut_implements_IUserReader()
    {
        typeof(ShopUserReader).Should().Implement<IUserReader>();
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_user_with_matching_name(
        [Frozen] Func<SellersDbContext> contextFactory,
        ShopUserReader sut,
        Shop shop,
        string password,
        IPasswordHasher hasher)
    {
        // Arrange
        using SellersDbContext context = contextFactory.Invoke();
        shop.PasswordHash = hasher.HashPassword(password);
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        // Act
        User? actual = await sut.FindUser(username: shop.UserId);

        // Assert
        actual.Should().NotBeNull();
        actual!.Id.Should().Be(shop.Id);
        actual.Username.Should().Be(shop.UserId);
        actual.PasswordHash.Should().Be(shop.PasswordHash);
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_null_with_nonexistent_username(
        ShopUserReader sut,
        string username)
    {
        User? actual = await sut.FindUser(username);
        actual.Should().BeNull();
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_user_with_matching_id(
        [Frozen] Func<SellersDbContext> contextFactory,
        ShopUserReader sut,
        Shop shop)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        User? actual = await sut.FindUser(id: shop.Id);

        actual.Should().BeEquivalentTo(new
        {
            Id = shop.Id,
            Username = shop.UserId,
            shop.PasswordHash,
        }, c => c.ExcludingMissingMembers());
    }

    [Theory, AutoSellersData]
    public async Task Sut_correctly_sets_roles(
        [Frozen] Func<SellersDbContext> contextFactory,
        ShopUserReader sut,
        Shop shop)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        User? user = await sut.FindUser(id: shop.Id);
        ImmutableArray<Role> actual = user!.Roles;

        Role role = new(shop.Id, RoleName: "Administrator");
        actual.Should().BeEquivalentTo(new[] { role });
    }
}
