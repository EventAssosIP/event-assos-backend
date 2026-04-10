using EventAssos.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class UpdateMemberRequestDTO
    {
        [Required(ErrorMessage = "Email is required !")]
        [EmailAddress(ErrorMessage = "Invalid format !")]
        public required string EmailAddress { get; set; }

        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?=&])[A-Za-z\d@$!%*?=&]{8,}$",
            ErrorMessage = "Password must contain at least 8 characters, one uppercase letter and one number !")]
        public required string Password { get; set; }

        [StringLength(25, ErrorMessage = "Pseudonim must contain at least 2 and maximum 25 characters !", MinimumLength = 2)]
        public string Pseudo { get; set; } = null!;

        public string Birthdate { get; set; } = null!;

        public string Gender { get; set; } = null!;
    }
}
