using Microsoft.EntityFrameworkCore;

namespace Sellers;

#nullable disable

public sealed class SellersDbContext : DbContext
{
    public SellersDbContext(DbContextOptions<SellersDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<RoleEntity> Roles { get; set; }

    public DbSet<Shop> Shops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(user =>
        {
            user.HasKey(x => x.Sequence);
            user.Property(x => x.Sequence).ValueGeneratedOnAdd();
            user.HasIndex(x => x.Id).IsUnique();
            user.HasIndex(x => x.Username).IsUnique();
        });

        modelBuilder.Entity<RoleEntity>(role =>
        {
            role.HasKey(x => new { x.UserSequence, x.ShopId, x.RoleName });
        });

        modelBuilder.Entity<Shop>(shop =>
        {
            shop.HasKey(x => x.Sequence);
            shop.Property(x => x.Sequence).ValueGeneratedOnAdd();
            shop.HasIndex(x => x.Id).IsUnique();
            shop.HasIndex(x => x.Name).IsUnique();
            shop.HasIndex(x => x.UserId).IsUnique();
        });
    }
}

#nullable enable
