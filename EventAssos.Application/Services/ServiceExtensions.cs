using EventAssos.Application.Interfaces.Services;
using EventAssos.Application.Interfaces.Services.Auth;
using EventAssos.Application.Interfaces.Services.Tools;
using EventAssos.Application.Services.Auth;
using EventAssos.Application.Services.Data;
using EventAssos.Application.Services.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssos.Application.Services;

public static class ServiceExtensions
{
    public static void ConfigureCore(this IServiceCollection services)
    {
        // Ajouter toutes les configurations liées au Core (ex: Services, etc.)
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasherService, PasswordHasherService>();
        services.AddScoped<IJwtService, JwtService>();
    }  
}
