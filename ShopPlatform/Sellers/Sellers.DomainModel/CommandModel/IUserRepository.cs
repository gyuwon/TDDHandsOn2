namespace Sellers.CommandModel;

public interface IUserRepository
{
    Task Add(User user);

    Task<bool> TryUpdate(Guid id, Func<User, User> reviser);
}
