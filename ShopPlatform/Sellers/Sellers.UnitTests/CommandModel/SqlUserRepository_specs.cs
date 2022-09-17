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

    [Theory, AutoSellersData]
    public async Task TryUpdate_returns_true_if_entity_exists(
        SqlUserRepository sut,
        User user)
    {
        await sut.Add(user);
        bool actual = await sut.TryUpdate(user.Id, x => x);
        actual.Should().BeTrue();
    }

    [Theory, AutoSellersData]
    public async Task TryUpdate_correctly_restores_user(
        SqlUserRepository sut,
        User user)
    {
        await sut.Add(user);
        User? actual = null;

        await sut.TryUpdate(user.Id, x => actual = x);

        actual.Should().BeEquivalentTo(user);
    }

    [Theory, AutoSellersData]
    public async Task TryUpdate_correctly_changes_username(
        [Frozen] Func<SellersDbContext> contextFactory,
        SqlUserRepository sut,
        User user,
        string newValue)
    {
        await sut.Add(user);

        await sut.TryUpdate(user.Id, x => x with { Username = newValue });

        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        actual.Username.Should().Be(newValue);
    }

    [Theory, AutoSellersData]
    public async Task TryUpdate_correctly_changes_password_hash(
        [Frozen] Func<SellersDbContext> contextFactory,
        SqlUserRepository sut,
        User user,
        string newValue)
    {
        await sut.Add(user);

        await sut.TryUpdate(user.Id, x => x with { PasswordHash = newValue });

        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        actual.PasswordHash.Should().Be(newValue);
    }

    [Theory, AutoSellersData]
    public async Task TryUpdate_correctly_changes_roles(
        [Frozen] Func<SellersDbContext> contextFactory,
        SqlUserRepository sut,
        User user,
        Role newRole)
    {
        await sut.Add(user);

        await sut.TryUpdate(user.Id, x => x with
        {
            Roles = x.Roles.Add(newRole),
        });

        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        actual.Roles.Should().Contain(user.Roles).And.Contain(newRole);
    }
}
