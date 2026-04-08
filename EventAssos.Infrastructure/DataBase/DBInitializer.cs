using EventAssos.Infrastructure.DataBase.Context;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using EventAssos.Application.Services.Tools;

namespace EventAssos.Infrastructure.DataBase
{
    public static class DbInitializer
    {
        public static void Seed(EventAssosContext context)
        {
            // S'assure que la base de données est créée
            context.Database.EnsureCreated();

            // --- 1. SEED DES MEMBRES ---
            if (!context.Members.Any())
            {
                var hasher = new PasswordHasherService();

                // On génère le hash Argon2id pour le mot de passe de test
                // Ce hash contient le sel et le hash combinés en Base64
                string hashedResult = hasher.HashPassword("Admin123!");

                context.Members.AddRange(
                    new Member
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        Pseudo = "AdminMaster",
                        EmailAddress = EmailAddress.Create("admin@test.com"),
                        // Utilisation du constructeur du VO PasswordHash
                        Password = new PasswordHash(hashedResult),
                        Birthdate = new DateOnly(1990, 1, 1),
                        Gender = Gender.Other,
                        Role = Role.Admin,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Member
                    {
                        Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                        Pseudo = "JeanTest",
                        EmailAddress = EmailAddress.Create("user@test.com"),
                        Password = new PasswordHash(hashedResult),
                        Birthdate = new DateOnly(1995, 5, 15),
                        Gender = Gender.Male,
                        Role = Role.User,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            // --- 2. SEED DES ÉVÉNEMENTS ---
            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event
                    {
                        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        Name = "Atelier .NET Core & Argon2id",
                        Description = "Apprendre la sécurité et l'isolation des transactions.",
                        Location = "Namur, Belgique",
                        StartDate = DateTime.UtcNow.AddDays(15),
                        EndDate = DateTime.UtcNow.AddDays(15).AddHours(4),
                        Category = Category.Workshop,
                        MinParticipants = 5,
                        MaxParticipants = 20,
                        Status = EventStatus.InProgress,
                        IsWaitingListActive = true,
                        RegistrationDeadline = DateTime.UtcNow.AddDays(10),
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Event
                    {
                        Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                        Name = "Conférence Architecture Pro",
                        Description = "Session complète pour tester la liste d'attente.",
                        Location = "En ligne",
                        StartDate = DateTime.UtcNow.AddDays(5),
                        EndDate = DateTime.UtcNow.AddDays(5).AddHours(2),
                        Category = Category.Conference,
                        MinParticipants = 1,
                        MaxParticipants = 1, // On limite à 1 pour simuler un événement plein
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