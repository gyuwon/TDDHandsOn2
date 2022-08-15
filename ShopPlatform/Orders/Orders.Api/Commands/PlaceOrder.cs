namespace Orders.Commands;

public sealed record PlaceOrder(
    Guid UserId,
    Guid ShopId,
    Guid ItemId,
    decimal Price);
