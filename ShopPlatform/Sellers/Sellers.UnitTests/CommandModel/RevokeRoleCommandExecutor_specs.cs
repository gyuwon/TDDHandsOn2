using FluentAssertions;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class RevokeRoleCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_removes_role(
        Func<SellersDbContext> contextFactory,
        User user)
    {
        // Arrange
        SqlUserRepository repository = new(contextFactory);
        await repository.Add(user);
        RevokeRoleCommandExecutor sut = new(repository);
        Role role = user.Roles.Sample();

        // Act
        await sut.Execute(user.Id, new(role.ShopId, role.RoleName));

        // Assert
        SqlUserReader reader = new(contextFactory);
        User actual = (await reader.FindUser(user.Id))!;
        actual.Roles.Should().NotContain(role);
    }
}
