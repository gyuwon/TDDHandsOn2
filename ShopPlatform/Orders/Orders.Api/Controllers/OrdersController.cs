using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Commands;
using Orders.Events;
using Orders.Messaging;

namespace Orders.Controllers;

[Route("api/orders")]
public sealed class OrdersController : Controller
{
    [HttpGet]
    [Produces("application/json", Type = typeof(Order[]))]
    public async Task<IEnumerable<Order>> GetOrders(
        [FromQuery(Name = "user-id")] Guid? userId,
        [FromQuery(Name = "shop-id")] Guid? shopId,
        [FromServices] OrdersDbContext context)
    {
        return await context.Orders
            .AsNoTracking()
            .FilterByUser(userId)
            .FilterByShop(shopId)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    [Produces("application/json", Type = typeof(Order))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> FindOrder(
        Guid id,
        [FromServices] OrdersDbContext context,
        [FromServices] SellersService service)
    {
        if (await context.Orders.FindOrder(id) is Order order)
        {
            Shop shop = await service.GetShop(order.ShopId);
            order.ShopName = shop.Name;
            return Ok(order);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPost("{id}/place-order")]
    public async Task<IActionResult> PlaceOrder(
        Guid id,
        [FromBody] PlaceOrder command,
        [FromServices] OrdersDbContext context,
        [FromServices] SellersService sellers)
    {
        if (await sellers.FindShop(command.ShopId) is not null)
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
            await context.SaveChangesAsync();
            return Ok();
        }
        else
        {
            return BadRequest();
        }
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

        if (order.Status != OrderStatus.Pending)
        {
            return BadRequest();
        }

        order.Status = OrderStatus.AwaitingPayment;
        order.PaymentTransactionId = command.PaymentTransactionId;
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

        if (order.Status != OrderStatus.AwaitingPayment)
        {
            return BadRequest();
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

        if (order.Status != OrderStatus.AwaitingShipment)
        {
            return BadRequest();
        }

        order.Status = OrderStatus.Completed;
        order.ShippedAtUtc = listenedEvent.EventTimeUtc;
        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("accept/payment-approved")]
    public async Task<IActionResult> AcceptPaymentApproved(
        [FromBody] ExternalPaymentApproved externalEvent,
        [FromServices] IBus<PaymentApproved> bus)
    {
        await bus.Send(new(externalEvent.tid, externalEvent.approved_at));
        return Accepted();
    }
}
