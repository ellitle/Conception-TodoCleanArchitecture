using CleanTodo.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CleanTodo.Infrastructure.Repositories;
using CleanTodo.Domain.Interfaces.Gateways;
using CleanTodo.Infrastructure.Configuration;
using CleanTodo.Infrastructure.Gateways;
using System.Net.Http.Headers;

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
            ?? throw new InvalidOperationException("Section Broker manquante.");
        services.AddSingleton(brokerConfiguration);
        services.AddHttpClient<IBrokerGateway, BrokerGateway>(client =>
        {
            if (string.IsNullOrWhiteSpace(brokerConfiguration.BaseUrl))
                throw new InvalidOperationException("Broker:BaseUrl manquant.");

            if (string.IsNullOrWhiteSpace(brokerConfiguration.ClientId))
                throw new InvalidOperationException("Broker:ClientId manquant.");

            if (string.IsNullOrWhiteSpace(brokerConfiguration.AppToken))
                throw new InvalidOperationException("Broker:AppToken manquant.");

            client.BaseAddress = new Uri(brokerConfiguration.BaseUrl.TrimEnd('/') + "/");
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("x-client-id", brokerConfiguration.ClientId);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", brokerConfiguration.AppToken);
        });

        // Register Repositories
        services.AddScoped<ITodoRepository, TodoRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IShipRepository, ShipRepository>();
        return services;
    }
}
