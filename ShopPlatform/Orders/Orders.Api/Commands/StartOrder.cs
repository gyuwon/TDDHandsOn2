namespace Orders.Commands;

public sealed record StartOrder(string? PaymentTransactionId = null);
