using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class UpdateEventRequestDTO
    {
        [Required(ErrorMessage = "Name is required !")]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int MinParticipants { get; set; }

        public int MaxParticipants { get; set; }

        public bool WaitingListActive { get; set; }

        public DateTime RegistrationDeadline { get; set; }
    }
}