using EventAssos.Application.Services.Tools;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.DataBase
{
    public static class DbInitializer
    {
        public static void Seed(EventAssosContext context)
        {
            // IMPORTANT : Utiliser EnsureDeleted/Created uniquement en DEV 
            // pour repartir d'une base propre si tes tests ne s'affichent pas.
            // context.Database.EnsureDeleted(); 
            context.Database.Migrate();

            if (!context.Members.Any())
            {
                var hasher = new PasswordHasherService();
                string hash = hasher.HashPassword("Admin123!");

                context.Members.AddRange(
                    new Member
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Pseudo = "AdminMaster",
                        EmailAddress = EmailAddress.Create("admin@test.com"),
                        Password = new PasswordHash(hash),
                        Birthdate = new DateOnly(1990, 1, 1),
                        Role = Role.Admin,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Member
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Pseudo = "JeanTest",
                        EmailAddress = EmailAddress.Create("user@test.com"),
                        Password = new PasswordHash(hash),
                        Birthdate = new DateOnly(1995, 5, 15),
                        Role = Role.User,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Atelier .NET",
                        Description = "Ouvert aux inscriptions",
                        Location = "Namur",
                        StartDate = DateTime.UtcNow.AddDays(10),
                        EndDate = DateTime.UtcNow.AddDays(10).AddHours(2),
                        Category = Category.Workshop,
                        MinParticipants = 1,
                        MaxParticipants = 10,
                        Status = EventStatus.InProgress,
                        IsWaitingListActive = true,
                        RegistrationDeadline = DateTime.UtcNow.AddDays(9),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Event
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Conférence (Full)",
                        Description = "Pour tester la liste d'attente",
                        Location = "Online",
                        StartDate = DateTime.UtcNow.AddDays(5),
                        EndDate = DateTime.UtcNow.AddDays(5).AddHours(1),
                        Category = Category.Conference,
                        MinParticipants= 1,
                        MaxParticipants = 1, // Limité à 1 !
                        Status = EventStatus.InProgress,
                        IsWaitingListActive = true,
                        RegistrationDeadline = DateTime.UtcNow.AddDays(4),
                        UpdatedAt = DateTime.UtcNow
                    }
                );
            }

            context.SaveChanges();
        }
    }
}