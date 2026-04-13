using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class CreateEventRequestDTO
    {
        [Required(ErrorMessage = "Le nom de l'événement est obligatoire.")]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Le lieu est obligatoire.")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de début est obligatoire.")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "La catégorie est obligatoire.")]
        public Guid CategoryId { get; set; }

        [Range(1, int.MaxValue)]
        public int MinParticipants { get; set; } = 1;

        public int? MaxParticipants { get; set; }

        public bool WaitingListActive { get; set; } = false;

        public DateTime? RegistrationDeadline { get; set; }
    }
}

