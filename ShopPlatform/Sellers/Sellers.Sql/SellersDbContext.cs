using Microsoft.EntityFrameworkCore;

namespace Sellers;

#nullable disable

public sealed class SellersDbContext : DbContext
{
    public SellersDbContext(DbContextOptions<SellersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Shop> Shops { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
