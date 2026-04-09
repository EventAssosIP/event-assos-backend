using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
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
            services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();

            // EmailService
            var smtpSection = configuration.GetSection("Smtp");

            // On vérifie que la section existe pour éviter un crash silencieux
            if (smtpSection.Exists())
            {
                services.AddScoped<IEmailService>(sp =>
                    new SmtpEmailService(
                        smtpSection["Host"] ?? "localhost",
                        int.Parse(smtpSection["Port"] ?? "587"),
                        smtpSection["User"] ?? "",
                        smtpSection["Pass"] ?? "",
                        smtpSection["FromEmail"] ?? "no-reply@eventassos.com",
                        smtpSection["FromName"] ?? "Event Assos"
                    )
                );
            }
        }
    }
}