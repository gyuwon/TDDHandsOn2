using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sellers.QueryModel;
using Xunit;

namespace Sellers;

public class Program_specs
{
    [Theory, AutoSellersData]
    public void Sut_registers_correct_service_of_IUserReader(
        SellersServer server)
    {
        IUserReader actual = server.Services.GetRequiredService<IUserReader>();
        actual.Should().BeOfType<BackwardCompatibleUserReader>();
    }
}
