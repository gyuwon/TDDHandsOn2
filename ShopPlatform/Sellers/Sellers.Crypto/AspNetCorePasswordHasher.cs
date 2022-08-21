using Microsoft.AspNetCore.Identity;

namespace Sellers;

public sealed class AspNetCorePasswordHasher : IPasswordHasher
{
    private static readonly object User = "user";

    private readonly PasswordHasher<object> hasher;

    public AspNetCorePasswordHasher(PasswordHasher<object> hasher)
        => this.hasher = hasher;

    public string HashPassword(string password)
        => hasher.HashPassword(User, password);

    public bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        return hasher.VerifyHashedPassword(User, hashedPassword, providedPassword) switch
        {
            PasswordVerificationResult.Failed => false,
            _ => true,
        };
    }
}
