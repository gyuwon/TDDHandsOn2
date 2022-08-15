namespace Orders.Events;

public sealed record ItemShipped(Guid OrderId, DateTime EventTimeUtc);
