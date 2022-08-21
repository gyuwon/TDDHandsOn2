namespace Sellers.CommandModel;

public sealed class SqlUserRepository : IUserRepository
{
    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserRepository(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public async Task Add(User user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
        });
        await context.SaveChangesAsync();
    }
}
