using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sellers;

#nullable disable

public sealed class Shop
{
    public Guid Id { get; set; }

    [JsonIgnore]
    public int Sequence { get; set; }

    [Required]
    public string Name { get; set; }

    public string UserId { get; set; }

    public string PasswordHash { get; set; }
}

#nullable enable
