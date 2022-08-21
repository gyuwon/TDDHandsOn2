namespace Sellers.QueryModel;

public sealed class PasswordVerifier
{
    public PasswordVerifier(IUserReader reader, IPasswordHasher hasher)
    {
    }

    public Task<bool> VerifyPassword(string username, string password)
    {
        return Task.FromResult<bool>(default);
    }
}
