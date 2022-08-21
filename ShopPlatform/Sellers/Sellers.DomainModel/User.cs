namespace Sellers;

public sealed record User(Guid Id, string Username, string PasswordHash);
