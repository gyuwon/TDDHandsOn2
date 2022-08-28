namespace Sellers.QueryModel;

public interface IUserReader
{
    Task<User?> FindUser(Guid id);

    Task<User?> FindUser(string username);
}
