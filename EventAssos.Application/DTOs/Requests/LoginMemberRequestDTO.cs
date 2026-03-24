using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class LoginMemberRequestDTO
    {
    [Required(ErrorMessage = "L'email est requise.")]
    public required EmailAddress EmailAddress { get; set; }

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public required PasswordHash Password { get; set; }
    }
}