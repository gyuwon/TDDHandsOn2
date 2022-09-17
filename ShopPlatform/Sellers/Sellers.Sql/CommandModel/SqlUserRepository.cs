using AutoMapper;
using System.Collections.Immutable;

namespace Sellers.CommandModel;

public sealed class SqlUserRepository : IUserRepository
{
    private static readonly IMapper mapper = new MapperConfiguration(c =>
    {
        c.CreateMap<Role, RoleEntity>();
        c.CreateMap<User, UserEntity>();
        c.CreateMap<RoleEntity, Role>();
        c.CreateMap<UserEntity, User>();
        c.CreateMap<List<RoleEntity>, ImmutableArray<Role>>().ConvertUsing(x => ConvertRoles(x));
    }).CreateMapper();

    private static ImmutableArray<Role> ConvertRoles(List<RoleEntity> roles)
        => roles.Select(x => mapper.Map<Role>(x)).ToImmutableArray();

    private readonly Func<SellersDbContext> contextFactory;

    public SqlUserRepository(Func<SellersDbContext> contextFactory)
        => this.contextFactory = contextFactory;

    public async Task Add(User user)
    {
        using SellersDbContext context = contextFactory.Invoke();
        context.Users.Add(mapper.Map<UserEntity>(user));
        await context.SaveChangesAsync();
    }

    public async Task<bool> TryUpdate(Guid id, Func<User, User> reviser)
    {
        using SellersDbContext context = contextFactory.Invoke();
        if (await context.FindUser(id) is UserEntity entity)
        {
            User revision = reviser.Invoke(mapper.Map<User>(entity));
            mapper.Map(revision, entity, typeof(User), typeof(UserEntity));
            await context.SaveChangesAsync();
            return true;
        }
        else
        {
            return false;
        }
    }
}
