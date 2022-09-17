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
    public async Task<IActionResult> GetRoles(
        Guid id,
        [FromServices] IUserReader reader)
    {
        return await reader.FindUser(id) switch
        {
            User user => Ok(user.Roles),
            _ => NotFound()
        };
    }

    [HttpPost("{id}/grant-role")]
    [ProducesResponseType(200)]
    public Task GrantRole(
        Guid id,
        [FromBody] GrantRole command,
        [FromServices] GrantRoleCommandExecutor executor)
    {
        return executor.Execute(id, command);
    }

    [HttpPost("{id}/revoke-role")]
    [ProducesResponseType(200)]
    public Task RevokeRole(
        Guid id,
        [FromBody] RevokeRole command,
        [FromServices] RevokeRoleCommandExecutor executor)
    {
        return executor.Execute(id, command);
    }
}
