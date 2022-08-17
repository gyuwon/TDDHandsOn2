using Azure.Storage.Queues;
using Microsoft.EntityFrameworkCore;
using Orders.Events;
using Orders.Messaging;

namespace Orders;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IServiceCollection services = builder.Services;

        services.AddDbContext<OrdersDbContext>(ConfigureDbContextOptions);

        services.AddSingleton(provider => new StorageQueueBus(GetQueueClient(provider)));
        services.AddSingleton<IBus<PaymentApproved>>(GetQueueBus);
        services.AddSingleton<IAsyncObservable<PaymentApproved>>(GetQueueBus);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        app.MapControllers();

        PaymentApprovedEventHandler.Listen(app.Services);

        app.Run();
    }

    private static void ConfigureDbContextOptions(
        IServiceProvider provider,
        DbContextOptionsBuilder options)
    {
        IConfiguration config = provider.GetRequiredService<IConfiguration>();
        options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
    }

    private static QueueClient GetQueueClient(IServiceProvider provider)
    {
        IConfiguration config = provider.GetRequiredService<IConfiguration>();
        return new(
            config["Storage:ConnectionString"],
            config["Storage:Queues:PaymentApproved"]);
    }

    private static StorageQueueBus GetQueueBus(IServiceProvider provider)
        => provider.GetRequiredService<StorageQueueBus>();
}
