using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sellers.QueryModel;

public sealed class ShopUserReader : IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public ShopUserReader(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public Task<User?> FindUser(Guid id)
        => FindUser(x => x.Id == id);

    public Task<User?> FindUser(string username)
        => FindUser(x => x.UserId == username);

    private async Task<User?> FindUser(Expression<Func<Shop, bool>> predicate)
    {
        using SellersDbContext context = contextFactory.Invoke();
        IQueryable<Shop> query = context.Shops.AsNoTracking().Where(predicate);
        return await query.SingleOrDefaultAsync() switch
        {
            Shop shop => new User(
                Id: shop.Id,
                Username: shop.UserId,
                shop.PasswordHash,
                Roles: ImmutableArray<Role>.Empty),
            null => null,
        };
    }
}
