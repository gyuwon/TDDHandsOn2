namespace Orders;

#nullable disable

public sealed class Order
{
    public Guid Id { get; set; }

    public long Sequence { get; set; }

    public Guid UserId { get; set; }

    public Guid ShopId { get; set; }

    public Guid ItemId { get; set; }

    public decimal Price { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime PlacedAtUtc { get; set; }

    public DateTime? StartedAtUtc { get; set; }

    public DateTime? PaidAtUtc { get; set; }

    public DateTime? ShippedAtUtc { get; set; }
}

#nullable enable