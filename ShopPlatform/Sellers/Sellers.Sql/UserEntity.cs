using System.ComponentModel.DataAnnotations;

namespace Sellers;

#nullable disable

public sealed class UserEntity
{
    public Guid Id { get; set; }

    public long Sequence { get; set; }

    [Required]
    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public List<RoleEntity> Roles { get; set; } = new();
}

#nullable enable
