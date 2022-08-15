namespace Orders;

internal static class QueryLanguage
{
    public static IQueryable<Order> FilterByUser(
        this IQueryable<Order> query,
        Guid? userId)
    {
        return userId == null ? query : query.Where(x => x.UserId == userId);
    }

    public static IQueryable<Order> FilterByShop(
        this IQueryable<Order> query,
        Guid? shopId)
    {
        return shopId == null ? query : query.Where(x => x.ShopId == shopId);
    }
}
