using FluentAssertions;
using Sellers.Commands;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Sellers.api.users.id.create_user;

public class Post_specs
{
    [Theory, AutoSellersData]
    public async Task Sut_correctly_executes_command(
        SellersServer server,
        Guid userId,
        CreateUser command)
    {
        using HttpClient client = server.CreateClient();
        string commandUri = $"api/users/{userId}/create-user";
        await client.PostAsJsonAsync(commandUri, command);

        string queryUri = "api/users/verify-password";
        var credentials = new { command.Username, command.Password };
        HttpResponseMessage response = await client.PostAsJsonAsync(queryUri, credentials);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory, AutoSellersData]
    public async Task Sut_returns_BadRequest_for_duplicate_usernameAsync(
        SellersServer server,
        Guid userId1,
        Guid userId2,
        CreateUser command)
    {
        using HttpClient client = server.CreateClient();
        string commandUri1 = $"api/users/{userId1}/create-user";
        await client.PostAsJsonAsync(commandUri1, command);

        string commandUri2 = $"api/users/{userId2}/create-user";
        HttpResponseMessage response = await client.PostAsJsonAsync(commandUri2, command);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
