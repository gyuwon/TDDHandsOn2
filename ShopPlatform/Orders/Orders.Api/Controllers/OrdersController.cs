using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Commands;
using Orders.Events;

namespace Orders.Controllers;

[Route("api/orders")]
public sealed class OrdersController : Controller
{
    [HttpGet]
    [Produces("application/json", Type = typeof(Order[]))]
    public async Task<IEnumerable<Order>> GetOrders(
        [FromServices] OrdersDbContext context)
    {
        return await context.Orders.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id}")]
    [Produces("application/json", Type = typeof(Order))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> FindOrder(
        Guid id,
        [FromServices] OrdersDbContext context)
    {
        return await context.Orders.FindOrder(id) switch
        {
            Order order => Ok(order),
            null => NotFound(),
        };
    }

    [HttpPost("{id}/place-order")]
    public Task PlaceOrder(
        Guid id,
        [FromBody] PlaceOrder command,
        [FromServices] OrdersDbContext context)
    {
        context.Add(new Order
        {
            Id = id,
            UserId = command.UserId,
            ShopId = command.ShopId,
            ItemId = command.ItemId,
            Price = command.Price,
            Status = OrderStatus.Pending,
            PlacedAtUtc = DateTime.UtcNow,
        });
        return context.SaveChangesAsync();
    }

    [HttpPost("{id}/start-order")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> StartOrder(
        Guid id,
        [FromBody] StartOrder command,
        [FromServices] OrdersDbContext context)
    {
        Order? order = await context.Orders.FindOrder(id);

        if (order == null)
        {
            return NotFound();
        }

        order.Status = OrderStatus.AwaitingPayment;
        order.StartedAtUtc = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("handle/bank-transder-payment-completed")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> HandleBankTransferPaymentCompleted(
        [FromBody] BankTransferPaymentCompleted listenedEvent,
        [FromServices] OrdersDbContext context)
    {
        Order? order = await context.Orders.FindOrder(listenedEvent.OrderId);

        if (order == null)
        {
            return NotFound();
        }

        order.Status = OrderStatus.AwaitingShipment;
        order.PaidAtUtc = listenedEvent.EventTimeUtc;
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("handle/item-shipped")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> HandleItemShipped(
        [FromBody] ItemShipped listenedEvent,
        [FromServices] OrdersDbContext context)
    {
        Order? order = await context.Orders.FindOrder(listenedEvent.OrderId);

        if (order == null)
        {
            return NotFound();
        }

        order.Status = OrderStatus.Completed;
        order.ShippedAtUtc = listenedEvent.EventTimeUtc;
        await context.SaveChangesAsync();
        return Ok();
    }
}
