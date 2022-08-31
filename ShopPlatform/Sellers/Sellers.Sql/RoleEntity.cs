namespace Sellers;

#nullable disable

public sealed class RoleEntity
{
    public long UserSequence { get; set; }

    public Guid ShopId { get; set; }

    public string RoleName { get; set; }

    public UserEntity User { get; set; }
}

#nullable enable
