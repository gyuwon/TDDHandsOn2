using AutoFixture;

namespace Sellers;

public sealed class UserCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        ICustomization customization = fixture
            .Build<UserEntity>()
            .Without(x => x.Sequence)
            .ToCustomization();

        fixture.Customize(customization);
    }
}
