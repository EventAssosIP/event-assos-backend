using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class UpdateMemberRequestDTO
    {
        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress(ErrorMessage = "Invalid format !")]
        public EmailAddress EmailAddress { get; set; } = null!;

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?=&])[A-Za-z\d@$!%*?=&]{8,}$",
            ErrorMessage = "Password must contain at least 8 characters, one uppercase letter and one number !")]
        public string Password { get; set; } = null!;


        [StringLength(25, ErrorMessage = "Pseudonim must contain at least 2 and maximum 25 characters !", MinimumLength = 2)]
        public string? Pseudo { get; set; }
    }
}
