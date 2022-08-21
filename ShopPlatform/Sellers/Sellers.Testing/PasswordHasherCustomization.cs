using AutoFixture;
using Microsoft.AspNetCore.Identity;

namespace Sellers;

public sealed class PasswordHasherCustomization : ICustomization
{
    public void Customize(IFixture fixture)
        => fixture.Register(() => new PasswordHasher<object>());
}
