using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Infrastructure.DataBase.Context;
using EventAssos.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventAssos.Infrastructure
{
    public static class ServiceExtensions 
    {
        public static void ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Ajouter toutes les configurations liées à l'Infrastructure (ex: DbContext, Repositories, etc.)
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<EventAssosContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
        }
    }
}

