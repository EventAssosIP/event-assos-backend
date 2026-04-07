using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;

namespace EventAssos.Infrastructure.DataBase
{
    public static class DbInitializer
    {
        public static void Seed(EventAssosContext context)
        {
            // On ne seed que si la base est vide
            if (context.Events.Any()) return;

            var events = new List<Event>
        {
            new Event
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                Name = "Conférence Tech 2026",
                Description = "Formation avancée .NET",
                Location = "Bruxelles",
                StartDate = DateTime.UtcNow.AddMonths(1),
                EndDate = DateTime.UtcNow.AddMonths(1).AddHours(8),
                Category = Domain.Enums.Category.Conference, // Utilise tes Enums !
                MinParticipants = 5,
                MaxParticipants = 50,
                IsWaitingListActive = true,
                RegistrationDeadline = DateTime.UtcNow.AddMonths(1).AddDays(-5)
            }
        };

            context.Events.AddRange(events);
            context.SaveChanges();
        }
    }
}
