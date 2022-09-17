using FluentAssertions;
using Sellers.Commands;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class GrantRoleCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_adds_role_to_user(
        Func<SellersDbContext> contextFactory,
        User user,
        GrantRole command)
    {
        SqlUserRepository repository = new(contextFactory);
        await repository.Add(user);
        GrantRoleCommandExecutor sut = new(repository);

        await sut.Execute(user.Id, command);

        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        Role role = new(command.ShopId, command.RoleName);
        actual.Roles.Should().Contain(role);
    }

    [Theory, AutoSellersData]
    public async Task Sut_fails_if_entity_not_found(
        GrantRoleCommandExecutor sut,
        Guid userId,
        GrantRole command)
    {
        Func<Task> action = () => sut.Execute(userId, command);
        await action.Should().ThrowAsync<EntityNotFoundException>();
    }

    [Theory, AutoSellersData]
    public async Task Sut_is_idempotent(
        Func<SellersDbContext> contextFactory,
        User user,
        GrantRole command)
    {
        SqlUserRepository repository = new(contextFactory);
        await repository.Add(user);
        GrantRoleCommandExecutor sut = new(repository);

        await sut.Execute(user.Id, command);
        await sut.Execute(user.Id, command);

        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        Role role = new(command.ShopId, command.RoleName);
        actual.Roles.Should().Contain(role);
    }
}
