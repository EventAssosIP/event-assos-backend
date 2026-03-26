using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class RegisterMemberRequestDTO
    {
        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress(ErrorMessage = "Invalid format !")]
        public required EmailAddress EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required !")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?=&])[A-Za-z\d@$!%*?=&]{8,}$",
            ErrorMessage = "Password must contain at least 8 characters, one uppercase letter and one number !")]
        public required PasswordHash Password { get; set; }

        [Required(ErrorMessage = "Pseudonim is required !")]
        [StringLength(25, ErrorMessage = "Pseudonim must contain at least 2 and maximum 25 characters !", MinimumLength = 2)]
        public required string Pseudo { get; set; }
    }
}