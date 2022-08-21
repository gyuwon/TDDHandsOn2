namespace Sellers.QueryModel;

public sealed class BackwardCompatibleUserReader : IUserReader
{
    private readonly IEnumerable<IUserReader> readers;

    private BackwardCompatibleUserReader(params IUserReader[] readers)
        => this.readers = readers.ToList().AsReadOnly();

    public BackwardCompatibleUserReader(Func<SellersDbContext> contextFactory)
        : this(new SqlUserReader(contextFactory),
               new ShopUserReader(contextFactory))
    {
    }

    public async Task<User?> FindUser(string username)
    {
        return await readers
            .ToAsyncEnumerable()
            .SelectAwait(async x => await x.FindUser(username))
            .Where(x => x != null)
            .FirstOrDefaultAsync();
    }
}
