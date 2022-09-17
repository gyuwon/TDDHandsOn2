namespace Sellers.Commands;

public sealed record RevokeRole(Guid ShopId, string RoleName);
