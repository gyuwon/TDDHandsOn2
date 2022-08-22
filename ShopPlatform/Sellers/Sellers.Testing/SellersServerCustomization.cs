using AutoFixture;

namespace Sellers;

public sealed class SellersServerCustomization : ICustomization
{
    public void Customize(IFixture fixture)
        => fixture.Register(() => SellersServer.Create());
}
