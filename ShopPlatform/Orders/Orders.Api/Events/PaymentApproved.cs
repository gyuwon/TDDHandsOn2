namespace Orders.Events;

public sealed record PaymentApproved(
    string TransactionId,
    DateTime EventTimeUtc);
