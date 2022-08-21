using AutoFixture.Xunit2;
using FluentAssertions;
using Xunit;

namespace Sellers.QueryModel;

public class SqlUserReader_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_returns_user_with_matching_name(
        [Frozen] Func<SellersDbContext> contextFactory,
        SqlUserReader sut,
        UserEntity user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        User? actual = await sut.FindUser(user.Username);

        actual.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_null_for_unknown_name(
        SqlUserReader sut,
        string username)
    {
        User? actual = await sut.FindUser(username);
        actual.Should().BeNull();
    }
}
