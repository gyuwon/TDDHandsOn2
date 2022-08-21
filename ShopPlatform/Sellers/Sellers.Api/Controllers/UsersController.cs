using Microsoft.AspNetCore.Mvc;
using Sellers.QueryModel;

namespace Sellers.Controllers;

[Route("api/users")]
public class UsersController : Controller
{
    [HttpPost("verify-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult VerifyPassword(
        [FromBody] Credentials credentials,
        [FromServices] PasswordVerifier verifier)
    {
        return Ok();
    }
}
