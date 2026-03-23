using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class RegisterMemberRequestDTO
    {
        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "Le format est incorrect.")]
        public required EmailAddress Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?=&])[A-Za-z\d@$!%*?=&]{8,}$",
            ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial")]
        public required PasswordHash Password { get; set; }

        [Required(ErrorMessage = "Le pseudo est requis.")]
        [StringLength(25, ErrorMessage = "La longueur maximum doit être comprise entre 2 et 25 caractères", MinimumLength = 2)]
        public required string Pseudo { get; set; }
    }
}