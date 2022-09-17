using FluentAssertions;
using Xunit;

namespace Sellers.api.users.id.grant_role;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_adds_role_to_user(
        SellersServer server,
        Guid userId,
        string username,
        string password,
        Guid shopId,
        string roleName)
    {
        await server.CreateUser(userId, username, password);

        await server.GrantRole(userId, shopId, roleName);

        IEnumerable<Role> roles = await server.GetRoles(userId);
        roles.Should().Contain(new Role(shopId, roleName));
    }

}
