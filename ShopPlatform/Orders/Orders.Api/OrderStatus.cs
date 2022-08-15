using System.Text.Json.Serialization;

namespace Orders;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Pending,
    AwaitingPayment,
    AwaitingShipment,
    Completed,
}
