using Sellers.Commands;
using System.Collections.Immutable;

namespace Sellers;

public sealed record User(
    Guid Id,
    string Username,
    string PasswordHash,
    ImmutableArray<Role> Roles)
{
    internal User GrantRole(GrantRole command)
    {
        Role role = new(command.ShopId, command.RoleName);
        return Roles.Contains(role) ? this : this with
        {
            Roles = Roles.Add(role),
        };
    }
}
