using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Sellers.QueryModel;

public sealed class SqlUserReader : IUserReader
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserReader(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public async Task<User?> FindUser(string username)
    {
        using SellersDbContext context = contextFactory.Invoke();

        IQueryable<UserEntity> query =
            from x in context.Users.AsNoTracking()
            where x.Username == username
            select x;

        return await query.SingleOrDefaultAsync() switch
        {
            UserEntity user => new User(
                user.Id,
                user.Username,
                user.PasswordHash,
                ImmutableArray<Role>.Empty),
            null => null,
        };
    }
}
