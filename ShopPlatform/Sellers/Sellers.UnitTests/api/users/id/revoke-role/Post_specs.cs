using FluentAssertions;
using Xunit;

namespace Sellers.api.users.id.revoke_role;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correcty_removes_role_from_user(
        SellersServer server,
        Guid userId,
        string username,
        string password,
        Guid shopId,
        string roleName)
    {
        await server.CreateUser(userId, username, password);
        await server.GrantRole(userId, shopId, roleName);

        await server.RevokeRole(userId, shopId, roleName);

        IEnumerable<Role> roles = await server.GetRoles(userId);
        roles.Should().NotContain(new Role(shopId, roleName));
    }
}
