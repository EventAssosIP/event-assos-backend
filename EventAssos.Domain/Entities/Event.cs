using EventAssos.Domain.Enums;
using System.ComponentModel.DataAnnotations; // Nécessaire pour [Timestamp]

namespace EventAssos.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; } // FK

        public required string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public required DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public EventCategory Category { get; set; } = null!; // Navigation

        public required int MinParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public EventStatus Status { get; set; } = EventStatus.InProgress;

        public bool IsWaitingListActive { get; set; } = false;

        public DateTime RegistrationDeadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        // --- AJOUT POUR LA CONCURRENCE OPTIMISTE ---

        /// <summary>
        /// Jeton de version géré par SQL Server pour garantir l'isolation ACID (Optimistic Concurrency).
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

        /// <summary>
        /// Marque l'événement comme modifié pour forcer la mise à jour du RowVersion lors du SaveChanges.
        /// </summary>
        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        private readonly List<Registration> _registrations = new();
        public IReadOnlyCollection<Registration> Registrations => _registrations;
    }
}