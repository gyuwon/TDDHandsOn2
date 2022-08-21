using Microsoft.EntityFrameworkCore;

namespace Sellers;

public static class SellersDatabase
{
    public const string DefaultConnectionString = "Server=127.0.0.1;Port=5432;Database=Sellers_UnitTests;User Id=postgres;Password=mysecretpassword;";

    public static SellersDbContext CreateContext()
        => CreateContextUsingSqlServer();

    private static SellersDbContext CreateContextUsingSqlServer()
    {
        string connectionString = DefaultConnectionString;
        SellersDbContext context = new(GetSqlServerOptions(connectionString));
        context.Database.Migrate();
        return context;
    }

    private static DbContextOptions<SellersDbContext> GetSqlServerOptions(
        string connectionString)
    {
        DbContextOptionsBuilder<SellersDbContext> builder = new();
        return builder.UseNpgsql(connectionString).Options;
    }
}
