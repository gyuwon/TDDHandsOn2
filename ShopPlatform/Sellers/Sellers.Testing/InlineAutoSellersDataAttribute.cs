using AutoFixture.Xunit2;

namespace Sellers;

public class InlineAutoSellersDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoSellersDataAttribute(params object[] values)
        : base(new AutoSellersDataAttribute(), values)
    {
    }
}
