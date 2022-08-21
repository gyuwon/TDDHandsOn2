using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sellers.CommandModel;
using Sellers.QueryModel;

namespace Sellers;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IServiceCollection services = builder.Services;

        services.AddDbContext<SellersDbContext>(ConfigureDbContextOptions);
        services.AddSingleton(GetDbContextFactory);

        services.AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();
        services.AddSingleton<PasswordHasher<object>>();
        services.AddSingleton<IPasswordHasher, AspNetCorePasswordHasher>();
        services.AddSingleton<PasswordVerifier>();

        services.AddSingleton<IUserRepository, SqlUserRepository>();
        services.AddSingleton<CreateUserCommandExecutor>();
        services.AddSingleton<IUserReader, BackwardCompatibleUserReader>();

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
        app.Run();
    }

    private static void ConfigureDbContextOptions(
        IServiceProvider provider,
        DbContextOptionsBuilder options)
    {
        IConfiguration config = provider.GetRequiredService<IConfiguration>();
        options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
    }

    private static Func<SellersDbContext> GetDbContextFactory(
        IServiceProvider provider)
    {
        DbContextOptionsBuilder<SellersDbContext> options = new();
        ConfigureDbContextOptions(provider, options);
        return () => new SellersDbContext(options.Options);
    }
}
