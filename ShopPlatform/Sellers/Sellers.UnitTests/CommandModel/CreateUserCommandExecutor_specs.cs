using FluentAssertions;
using Sellers.Commands;
using Sellers.QueryModel;
using Xunit;

namespace Sellers.CommandModel;

public class CreateUserCommandExecutor_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_creates_user_entity(
        IPasswordHasher hasher,
        SqlUserReader reader,
        SqlUserRepository repository,
        Guid userId,
        CreateUser command)
    {
        CreateUserCommandExecutor sut = new(hasher, reader, repository);

        await sut.Execute(userId, command);

        User? actual = await reader.FindUser(command.Username);
        actual.Should().NotBeNull();
        actual!.Id.Should().Be(userId);
    }

    [Theory, AutoSellersData]
    public async Task Sut_correctly_hashes_password(
        IPasswordHasher hasher,
        SqlUserReader reader,
        SqlUserRepository repository,
        Guid userId,
        CreateUser command)
    {
        CreateUserCommandExecutor sut = new(hasher, reader, repository);

        await sut.Execute(userId, command);

        User? actual = await reader.FindUser(command.Username);
        string passwordHash = actual!.PasswordHash;
        hasher.VerifyPassword(passwordHash, command.Password).Should().BeTrue();
    }

    [Theory, AutoSellersData]
    public async Task Sut_fails_for_duplicate_username(
        IPasswordHasher hasher,
        SqlUserReader reader,
        SqlUserRepository repository,
        Guid userId1,
        Guid userId2,
        CreateUser command)
    {
        CreateUserCommandExecutor sut = new(hasher, reader, repository);
        await sut.Execute(userId1, command);

        Func<Task> action = () => sut.Execute(userId2, command);

        await action.Should().ThrowAsync<InvariantViolationException>();
    }
}
