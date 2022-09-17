namespace Sellers.CommandModel;

internal static class UserRepositoryExtensions
{
    public static async Task Update(
        this IUserRepository repository,
        Guid id,
        Func<User, User> reviser)
    {
        if (await repository.TryUpdate(id, reviser) == false)
        {
            throw new EntityNotFoundException();
        }
    }
}
