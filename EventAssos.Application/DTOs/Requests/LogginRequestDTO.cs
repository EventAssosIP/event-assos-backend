using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    internal class LogginRequestDTO
    {
    [Required(ErrorMessage = "L'email est requise.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public string Password { get; set; } = null!;
    }
}