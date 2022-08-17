using System.Diagnostics.CodeAnalysis;

namespace Orders.Events;

[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "External API")]
public sealed record ExternalPaymentApproved(string tid, DateTime approved_at);
