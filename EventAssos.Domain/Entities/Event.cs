using EventAssos.Domain.Enums;

namespace EventAssos.Domain.Entities
{
    public class Event
    {
        public Guid Id { get;  set; }

        public required string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public required DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Category Category { get; set; } = Category.Other;

        public required int MinParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public EventStatus Status { get; set; } = EventStatus.InProgress;

        public bool WaitingListActive { get; set; } = false;

        public DateTime RegistrationDeadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
     
}

