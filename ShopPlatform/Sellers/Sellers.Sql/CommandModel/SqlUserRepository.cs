using AutoMapper;

namespace Sellers.CommandModel;

public sealed class SqlUserRepository : IUserRepository
{
    private static readonly IMapper mapper = new MapperConfiguration(c =>
    {
        c.CreateMap<Role, RoleEntity>();
        c.CreateMap<User, UserEntity>();
    }).CreateMapper();

    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserRepository(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public async Task Add(User user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(mapper.Map<UserEntity>(user));
        await context.SaveChangesAsync();
    }
}
