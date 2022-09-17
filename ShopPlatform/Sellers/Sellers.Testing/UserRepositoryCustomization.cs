using AutoFixture;
using Sellers.CommandModel;

namespace Sellers;

public sealed class UserRepositoryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
        => fixture.Register<IUserRepository>(() => fixture.Create<SqlUserRepository>());
}
