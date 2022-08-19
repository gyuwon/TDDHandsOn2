using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sellers.Controllers;

[Route("api/shops")]
public sealed class ShopsControllers : Controller
{
    [HttpPost]
    [Produces("application/json", Type = typeof(Shop))]
    public async Task<IActionResult> CreateShop(
        [FromBody] Shop shop,
        [FromServices] SellersDbContext context)
    {
        shop.Id = Guid.NewGuid();
        context.Shops.Add(shop);
        await context.SaveChangesAsync();
        return Ok(shop);
    }

    [HttpGet]
    [Produces("application/json", Type = typeof(Shop[]))]
    public async Task<IEnumerable<Shop>> GetShops(
        [FromServices] SellersDbContext context)
    {
        return await context.Shops.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id}")]
    [Produces("application/json", Type = typeof(Shop))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> FindShop(
        Guid id,
        [FromServices] SellersDbContext context)
    {
        return await context.Shops.FindShop(id) switch
        {
            Shop shop => Ok(new ShopView(shop.Id, shop.Name)),
            null => NotFound(),
        };
    }

    [HttpPost("{id}/user")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> PostUser(
        Guid id,
        [FromBody] ShopUser user,
        [FromServices] SellersDbContext context,
        [FromServices] IPasswordHasher<object> hasher)
    {
        Shop? shop = await context.Shops.FindShop(id);
        if (shop == null)
        {
            return NotFound();
        }

        shop.UserId = user.Id;
        shop.PasswordHash = hasher.HashPassword(user, user.Password);

        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("verify-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> VerifyPassword(
        [FromBody] ShopUser user,
        [FromServices] SellersDbContext dbContext,
        [FromServices] IPasswordHasher<object> hasher)
    {
        IQueryable<Shop> query =
            from s in dbContext.Shops
            where s.UserId == user.Id
            select s;

        Shop? shop = await query.SingleOrDefaultAsync();

        if (shop == null)
        {
            return BadRequest();
        }
        else
        {
            if (shop.PasswordHash == null)
            {
                return BadRequest();
            }
            else
            {
                PasswordVerificationResult result = hasher.VerifyHashedPassword(
                    user,
                    shop.PasswordHash,
                    user.Password);

                return result == PasswordVerificationResult.Failed
                    ? BadRequest()
                    : Ok();
            }
        }
    }
}
