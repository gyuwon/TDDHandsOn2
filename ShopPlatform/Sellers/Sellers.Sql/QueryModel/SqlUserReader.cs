using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Sellers.QueryModel;

public sealed class SqlUserReader : IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserReader(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public Task<User?> FindUser(Guid id)
        => FindUser(x => x.Id == id);

    public Task<User?> FindUser(string username)
        => FindUser(x => x.Username == username);

    private async Task<User?> FindUser(
        Expression<Func<UserEntity, bool>> predicate)
    {
        using SellersDbContext context = contextFactory.Invoke();

        IQueryable<UserEntity> query = context.Users
            .AsNoTracking()
            .Where(predicate);

        return await query.Include(x => x.Roles).SingleOrDefaultAsync() switch
        {
            UserEntity entity => new User(
                entity.Id,
                entity.Username,
                entity.PasswordHash,
                Roles: ImmutableArray.CreateRange(
                    from x in entity.Roles
                    select new Role(x.ShopId, x.RoleName))),
            null => null,
        };
    }
}
