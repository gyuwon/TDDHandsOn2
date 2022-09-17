namespace Sellers;

internal static class CollectionExtensions
{
    private static readonly Random random = new();

    public static T Sample<T>(this IEnumerable<T> source)
    {
        IOrderedEnumerable<T> query =
            from x in source
            orderby random.Next()
            select x;

        return query.First();
    }
}
