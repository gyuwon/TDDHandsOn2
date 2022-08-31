using AutoFixture;

namespace Sellers;

public sealed class UserCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        ICustomization customization = fixture
            .Build<UserEntity>()
            .Without(x => x.Sequence)
            .With(x => x.Roles, new List<RoleEntity>())
            .ToCustomization();

        fixture.Customize(customization);
    }
}
