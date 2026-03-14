using Pasteleria.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ContextFactory
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>((serviceProvider, options) =>
        {
            options.UseSqlite(configuration.GetConnectionString("connection"));
            //options.UseMySql(configuration.GetConnectionString("connection"),
            //    new MySqlServerVersion(new Version(8, 0, 27)));
        });
    }
}
