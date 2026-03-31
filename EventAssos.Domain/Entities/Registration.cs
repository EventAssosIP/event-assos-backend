namespace EventAssos.Domain.Entities
{
    public class Registration
    {
        public Guid Id { get; set; }

        // FK
        public Guid EventId { get; set; }
        public Guid MemberId { get; set; }

        public DateTime RegisteredAt { get; set; }

        public bool IsConfirmed { get; set; }

        public int? WaitingPosition { get; set; }

        // Navigation properties
        public Event Event { get; set; } = null!;
        public Member Member { get; set; } = null!;
    }
}
