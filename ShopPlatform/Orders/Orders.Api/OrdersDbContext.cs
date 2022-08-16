using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Orders;

#nullable disable

public sealed class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        EntityTypeBuilder<Order> order = modelBuilder.Entity<Order>();
        order.HasKey(x => x.Sequence);
        order.Property(x => x.Sequence).ValueGeneratedOnAdd();
        order.HasIndex(x => x.Id).IsUnique();
        order.HasIndex(x => x.UserId);
        order.HasIndex(x => x.ShopId);
        order.Property(x => x.Status).HasConversion<string>();
        order.HasIndex(x => x.Status);
        order.HasIndex(x => x.PaymentTransactionId);
    }
}

#nullable enable
