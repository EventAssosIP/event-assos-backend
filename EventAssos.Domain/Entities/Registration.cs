namespace EventAssos.Domain.Entities
{
    public class Registration
    {
        public Guid Id { get; set; }

        // FK
        public Guid EventId { get; set; }
        public Guid MemberId { get; set; }

        public DateTime RegisteredAt { get; set; }

        // Pour gérer la waiting list
        // 0 ou null : membre actif
        // >0 : position dans la liste d'attente
        public int? WaitingPosition { get; set; }

        // Navigation properties optionnelles
        public Event Event { get; set; } = null!;
        public Member Member { get; set; } = null!;
    }
}
