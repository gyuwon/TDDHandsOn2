using AutoFixture;
using System.Reflection;

namespace Sellers;

[AttributeUsage(AttributeTargets.Parameter)]
public class ConnectionStringAttribute :
    Attribute,
    IParameterCustomizationSource,
    ICustomization
{
    public ConnectionStringAttribute(string connectionString)
        => ConnectionString = connectionString;

    public string ConnectionString { get; }

    public void Customize(IFixture fixture)
        => fixture.Register(() => SellersServer.Create(ConnectionString));

    public ICustomization GetCustomization(ParameterInfo parameter)
    {
        return parameter.ParameterType == typeof(SellersServer)
            ? this
            : new CompositeCustomization();
    }
}
