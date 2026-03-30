using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class AddEventRequestDTO
    {
        [Required(ErrorMessage = "Name is required !")]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required !")]
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue)]
        public int MinParticipants { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxParticipants { get; set; }

        public bool WaitingListActive { get; set; }

        [Required]
        public DateTime RegistrationDeadline { get; set; }
    }
}