using CleanTodo.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanTodo.Infrastructure.Repositories;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Infrastructure.Configuration;
using CleanTodo.Infrastructure.Gateways;

namespace CleanTodo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
        ));

        var brokerConfiguration = configuration.GetSection("Broker").Get<BrokerConfiguration>()
            ?? new BrokerConfiguration();
        services.AddSingleton(brokerConfiguration);
        services.AddHttpClient<IBrokerGateway, BrokerGateway>(client =>
        {
            client.BaseAddress = new Uri(brokerConfiguration.BaseUrl.TrimEnd('/') + "/");
        });

        // Register Repositories
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IShipRepository, ShipRepository>();
        services.AddScoped<IShipTransferRepository, ShipTransferRepository>();
        return services;
    }
}
