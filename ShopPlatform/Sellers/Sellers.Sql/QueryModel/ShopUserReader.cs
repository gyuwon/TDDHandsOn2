using Microsoft.EntityFrameworkCore;

namespace Sellers.QueryModel;

public sealed class ShopUserReader : IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public ShopUserReader(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public async Task<User?> FindUser(string username)
    {
        using SellersDbContext context = contextFactory.Invoke();

        IQueryable<Shop> query =
            from x in context.Shops.AsNoTracking()
            where x.UserId == username
            select x;

        return await query.SingleOrDefaultAsync() switch
        {
            Shop shop => new User(
                Id: shop.Id,
                Username: shop.UserId,
                shop.PasswordHash),
            null => null,
        };
    }
}
