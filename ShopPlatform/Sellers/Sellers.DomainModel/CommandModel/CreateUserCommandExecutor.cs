using Sellers.Commands;
using Sellers.QueryModel;

namespace Sellers.CommandModel;

public sealed class CreateUserCommandExecutor
{
    public CreateUserCommandExecutor(
        IPasswordHasher hasher,
        IUserReader reader,
        IUserRepository repository)
    {
    }

    public Task Execute(Guid id, CreateUser command)
    {
        return Task.CompletedTask;
    }
}
