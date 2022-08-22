using Sellers.Commands;
using Sellers.QueryModel;
using System.Collections.Immutable;

namespace Sellers.CommandModel;

public sealed class CreateUserCommandExecutor
{
    private readonly IPasswordHasher hasher;
    private readonly IUserReader reader;
    private readonly IUserRepository repository;

    public CreateUserCommandExecutor(
        IPasswordHasher hasher,
        IUserReader reader,
        IUserRepository repository)
    {
        this.hasher = hasher;
        this.reader = reader;
        this.repository = repository;
    }

    public async Task Execute(Guid id, CreateUser command)
    {
        await AssertThatUsernameIsAvailable(command.Username);
        await AddUser(id, command);
    }

    private async Task AssertThatUsernameIsAvailable(string username)
    {
        if (await reader.FindUser(username) is not null)
        {
            throw new InvariantViolationException();
        }
    }

    private Task AddUser(Guid id, CreateUser command)
    {
        string passwordHash = hasher.HashPassword(command.Password);
        ImmutableArray<Role> roles = ImmutableArray<Role>.Empty;
        return repository.Add(new(id, command.Username, passwordHash, roles));
    }
}
