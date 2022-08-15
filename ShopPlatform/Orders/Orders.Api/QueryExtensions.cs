using Microsoft.EntityFrameworkCore;

namespace Orders;

public static class QueryExtensions
{
    public static Task<Order?> FindOrder(this DbSet<Order> source, Guid id)
        => source.SingleOrDefaultAsync(x => x.Id == id);
}
