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

    public Task<User?> FindUser(Guid id)
        => FindUser(async x => await x.FindUser(id));

    public Task<User?> FindUser(string username)
        => FindUser(async x => await x.FindUser(username));

    private async Task<User?> FindUser(
        Func<IUserReader, ValueTask<User?>> selector)
    {
        return await readers
            .ToAsyncEnumerable()
            .SelectAwait(selector)
            .Where(x => x != null)
            .FirstOrDefaultAsync();
    }
}
