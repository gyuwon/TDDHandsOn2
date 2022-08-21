using FluentAssertions;
using Xunit;

namespace Sellers.QueryModel;

public class PasswordVerifier_specs
{
    [Theory]
    [InlineAutoSellersData("hello world", "hello world", true)]
    [InlineAutoSellersData("hello world", "yellow word", false)]
    public async Task Sut_correctly_checks_password(
        string password,
        string providedPassword,
        bool expected,
        string username,
        Func<SellersDbContext> contextFactory,
        IPasswordHasher hasher,
        Shop shop)
    {
        // Arrange
        PasswordVerifier sut = new(new ShopUserReader(contextFactory), hasher);

        using SellersDbContext context = contextFactory.Invoke();
        shop.UserId = username;
        shop.PasswordHash = hasher.HashPassword(password);
        context.Shops.Add(shop);
        await context.SaveChangesAsync();

        // Act
        bool actual = await sut.VerifyPassword(username, providedPassword);

        // Assert
        actual.Should().Be(expected);
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_false_for_unknown_username(
        Func<SellersDbContext> contextFactory,
        IPasswordHasher hasher,
        string username,
        string password)
    {
        PasswordVerifier sut = new(new ShopUserReader(contextFactory), hasher);
        bool actual = await sut.VerifyPassword(username, password);
        actual.Should().BeFalse();
    }
}
