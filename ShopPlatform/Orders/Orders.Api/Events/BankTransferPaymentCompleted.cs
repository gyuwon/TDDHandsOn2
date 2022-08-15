namespace Orders.Events;

public sealed record BankTransferPaymentCompleted(
    Guid OrderId,
    DateTime EventTimeUtc);
