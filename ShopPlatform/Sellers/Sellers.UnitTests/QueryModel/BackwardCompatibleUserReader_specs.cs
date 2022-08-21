using AutoFixture.Xunit2;
using FluentAssertions;
using System.Reflection;
using Xunit;

namespace Sellers.QueryModel;

public class BackwardCompatibleUserReader_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_correct_entity_with_username_from_user_model(
        [Frozen] Func<SellersDbContext> contextFactory,
        BackwardCompatibleUserReader sut,
        UserEntity user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        User? actual = await sut.FindUser(user.Username);

        actual.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_correct_entity_with_username_from_shop_model(
        [Frozen] Func<SellersDbContext> contextFactory,
        BackwardCompatibleUserReader sut,
        Shop shop)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        User? actual = await sut.FindUser(shop.UserId);

        actual.Should().BeEquivalentTo(new
        {
            shop.Id,
            Username = shop.UserId,
            shop.PasswordHash,
        });
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_null_for_unknown_name(
        BackwardCompatibleUserReader sut,
        string username)
    {
        User? actual = await sut.FindUser(username);
        actual.Should().BeNull();
    }
}
