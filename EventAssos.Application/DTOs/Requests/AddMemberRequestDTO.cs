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
        public EmailAddress? EmailAddress { get; private set; }

        [Required(ErrorMessage = "Password is required !")]
        public Password? Password { get; private set; }

        public DateTime Birthdate { get; private set; }

        public Gender Gender { get; private set; }
    }
}


