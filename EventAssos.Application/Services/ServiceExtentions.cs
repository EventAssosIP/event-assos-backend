using Microsoft.Extensions.DependencyInjection;


namespace EventAssos.Application.Services;

public static class ServiceExtensions
{
    public static void ConfigureCore(this IServiceCollection services)
    {
        // Ajouter toutes les configurations liées au Core (ex: Services, etc.)
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtService, JwtService>();
    }
}