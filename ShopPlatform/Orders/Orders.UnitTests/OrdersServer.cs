using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Orders;

public sealed class OrdersServer : TestServer
{
    private const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=Orders_UnitTests;User Id=postgres;Password=mysecretpassword;";

    public OrdersServer(
        IServiceProvider services,
        IOptions<TestServerOptions> optionsAccessor)
        : base(services, optionsAccessor)
    {
    }

    public static OrdersServer Create()
    {
        OrdersServer server = (OrdersServer)new Factory().Server;

        lock (typeof(OrdersDbContext))
        {
            using IServiceScope scope = server.Services.CreateScope();
            OrdersDbContext context = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            context.Database.Migrate();
        }

        return server;
    }

    private sealed class Factory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<IServer, OrdersServer>();
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = ConnectionString,
                });
            });

            return base.CreateHost(builder);
        }
    }
}
