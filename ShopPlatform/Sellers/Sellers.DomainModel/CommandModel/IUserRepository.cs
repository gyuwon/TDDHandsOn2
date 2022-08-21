namespace Sellers.CommandModel;

public interface IUserRepository
{
    Task Add(User user);
}
