using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    internal class AddMemberRequestDTO
    {
        [Required(ErrorMessage = "Pseudonim is required !")]
        public required string Pseudo { get; set; }

        [Required(ErrorMessage = "Email is required !")]
        public required string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required !")]
        public required string Password { get; set; }

        public DateTime Birthdate { get; private set; }

        public string Gender { get; private set; } = null!;
    }
}


