using Microsoft.AspNetCore.Mvc;
using Sellers.CommandModel;
using Sellers.Commands;
using Sellers.Filters;
using Sellers.QueryModel;

namespace Sellers.Controllers;

[Route("api/users")]
public class UsersController : Controller
{
    [HttpPost("verify-password")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> VerifyPassword(
        [FromBody] Credentials credentials,
        [FromServices] PasswordVerifier verifier)
    {
        (string username, string password) = credentials;
        return await verifier.VerifyPassword(username, password) switch
        {
            true => Ok(),
            _ => BadRequest(),
        };
    }

    [HttpPost("{id}/create-user")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [TypeFilter(typeof(InvariantViolationFilter))]
    public Task CreateUser(
        Guid id,
        [FromBody] CreateUser command,
        [FromServices] CreateUserCommandExecutor executor)
    {
        return executor.Execute(id, command);
    }

    [HttpGet("{id}/roles")]
    [ProducesResponseType(200, Type = typeof(Role[]))]
    [ProducesResponseType(404)]
    public void GetRoles(Guid id)
    {
    }
}
