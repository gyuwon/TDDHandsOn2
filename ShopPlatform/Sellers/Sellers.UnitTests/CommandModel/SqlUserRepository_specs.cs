using AutoFixture.Xunit2;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Sellers.QueryModel;
using System.Collections.Immutable;
using Xunit;

namespace Sellers.CommandModel;

public class SqlUserRepository_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_creates_new_entity(
        [Frozen] Func<SellersDbContext> contextFactory,
        SqlUserRepository sut,
        User source)
    {
        User user = source with { Roles = ImmutableArray<Role>.Empty };

        await sut.Add(user);

        using SellersDbContext context = contextFactory.Invoke();
        UserEntity? actual = await context.Users.SingleOrDefaultAsync(x => x.Id == user.Id);
        actual.Should().BeEquivalentTo(user, c => c.ExcludingMissingMembers());
    }

    [Theory, AutoSellersData]
    public async Task Sut_correctly_saves_roles(
        [Frozen] Func<SellersDbContext> contextFactory,
        SqlUserRepository sut,
        User user)
    {
        await sut.Add(user);

        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        actual.Roles.Should().BeEquivalentTo(user.Roles);
    }
}
