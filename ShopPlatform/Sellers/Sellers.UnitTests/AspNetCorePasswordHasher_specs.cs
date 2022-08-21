using FluentAssertions;
using Xunit;

namespace Sellers;

public class AspNetCorePasswordHasher_specs
{
    [Fact]
    public void Sut_implements_IPasswordHasher()
    {
        typeof(AspNetCorePasswordHasher).Should().Implement<IPasswordHasher>();
    }

    [Theory, AutoSellersData]
    public void HashPassword_returns_different_hash_each_time(
        AspNetCorePasswordHasher sut,
        string password)
    {
        Enumerable.Range(0, 100)
                  .Select(_ => sut.HashPassword(password))
                  .Should()
                  .OnlyHaveUniqueItems();
    }

    [Theory, AutoSellersData]
    public void VerifyPassword_returns_true_if_passwords_match(
        AspNetCorePasswordHasher sut,
        string password)
    {
        string hash = sut.HashPassword(password);
        bool actual = sut.VerifyPassword(hash, password);
        actual.Should().BeTrue();
    }

    [Theory, AutoSellersData]
    public void VerifyPassword_returns_false_if_passwords_not_match(
        AspNetCorePasswordHasher sut,
        string password,
        string wrongPassword)
    {
        string hash = sut.HashPassword(password);
        bool actual = sut.VerifyPassword(hash, wrongPassword);
        actual.Should().BeFalse();
    }
}
