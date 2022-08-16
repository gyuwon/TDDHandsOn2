using FluentAssertions;
using System.Net;
using Xunit;

namespace Orders.api.orders.handle.bank_transder_payment_completed;

public class Post_specs
{
    [Fact]
    public async Task Sut_fails_if_order_is_pending()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);

        HttpResponseMessage response = await
            server.HandleBankTransferPaymentCompleted(orderId);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_fails_if_payment_already_completed()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.HandleBankTransferPaymentCompleted(orderId);

        HttpResponseMessage response = await
            server.HandleBankTransferPaymentCompleted(orderId);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Sut_fails_if_order_completed()
    {
        OrdersServer server = OrdersServer.Create();
        Guid orderId = Guid.NewGuid();
        await server.PlaceOrder(orderId);
        await server.StartOrder(orderId);
        await server.HandleBankTransferPaymentCompleted(orderId);
        await server.HandleItemShipped(orderId);

        HttpResponseMessage response = await
            server.HandleBankTransferPaymentCompleted(orderId);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
