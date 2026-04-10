using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class UpdateEventRequestDTO
    {
        // On enlève 'required' et on autorise null pour la mise à jour partielle
        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? CategoryId { get; set; } // Changé en Guid pour correspondre à ton entité

        public int? MinParticipants { get; set; }

        public int? MaxParticipants { get; set; }

        public bool? WaitingListActive { get; set; }

        public DateTime? RegistrationDeadline { get; set; }
    }
}