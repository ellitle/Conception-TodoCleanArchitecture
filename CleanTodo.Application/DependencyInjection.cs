using CleanTodo.Application.Service;
using CleanTodo.Application.UseCase;
using CleanTodo.Application.UseCases;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanTodo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // cette ligne ajoute les validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<GetAllShipsUseCase>();
        services.AddScoped<GetShipUseCase>();
        services.AddScoped<GetPortUseCase>();
        services.AddScoped<CreateShipUseCase>();
        services.AddScoped<UpdateShipUseCase>();
        services.AddScoped<DeleteShipUseCase>();
        services.AddScoped<SailShipUseCase>();
        services.AddScoped<DockShipUseCase>();
        services.AddScoped<GetShipTransferUseCase>();

        services.AddScoped<GetAllTodosUseCase>();
        services.AddScoped<GetTodoUseCase>();
        services.AddScoped<CreateTodoUseCase>();
        services.AddScoped<UpdateTodoUseCase>();
        services.AddScoped<DeleteTodoUseCase>();

        services.AddScoped<JwtService>();
        services.AddScoped<PasswordHasher>();

        services.AddScoped<ToggleCompleteStatusTodoUseCase>();

        services.AddScoped<RegisterUserUseCase>();
        services.AddScoped<LoginUserUseCase>();
        
        return services;
    }
}
