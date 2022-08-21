namespace Sellers.QueryModel;

public sealed class PasswordVerifier
{
    private readonly IUserReader reader;
    private readonly IPasswordHasher hasher;

    public PasswordVerifier(IUserReader reader, IPasswordHasher hasher)
    {
        this.reader = reader;
        this.hasher = hasher;
    }

    public async Task<bool> VerifyPassword(string username, string password)
    {
        return await reader.FindUser(username) switch
        {
            User user => hasher.VerifyPassword(user.PasswordHash, password),
            _ => false,
        };
    }
}
