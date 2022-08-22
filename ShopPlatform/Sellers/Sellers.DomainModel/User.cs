using System.Collections.Immutable;

namespace Sellers;

public sealed record User(
    Guid Id,
    string Username,
    string PasswordHash,
    ImmutableArray<Role> Roles);
