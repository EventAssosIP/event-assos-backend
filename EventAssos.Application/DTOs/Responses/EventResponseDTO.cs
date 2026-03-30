namespace EventAssos.Application.DTOs.Responses
{
    public class EventResponseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Category {  get; set; } = string.Empty;

        public int MinParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public string EventStatus { get; set; } = string.Empty;

        public bool WaitingListActive { get; set; }

        public DateTime RegistrationDeadline { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}