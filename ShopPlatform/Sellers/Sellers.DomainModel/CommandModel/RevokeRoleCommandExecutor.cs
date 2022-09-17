using Sellers.Commands;

namespace Sellers.CommandModel;

public sealed class RevokeRoleCommandExecutor
{
    private readonly IUserRepository repository;

    public RevokeRoleCommandExecutor(IUserRepository repository)
        => this.repository = repository;

    public Task Execute(Guid id, RevokeRole command)
        => repository.Update(id, u => u.RevokeRole(command));
}
