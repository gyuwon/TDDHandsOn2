namespace Sellers.QueryModel;

public interface IUserReader
{
    Task<User?> FindUser(string username);
}
