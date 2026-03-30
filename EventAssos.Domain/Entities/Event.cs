using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Domain.Entities
{
    public class Event
    {
        public Guid Id { get;  set; }

        public required string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public required DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MinParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public required EventStatus Status { get; set; }

        public bool WaitingListActive { get; set; }

        public required DateTime RegistrationDeadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
     
}

