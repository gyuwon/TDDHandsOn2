using AutoFixture;

namespace Sellers;

public sealed class ShopCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        ICustomization customization = fixture
            .Build<Shop>()
            .Without(x => x.Sequence)
            .ToCustomization();

        fixture.Customize(customization);
    }
}
