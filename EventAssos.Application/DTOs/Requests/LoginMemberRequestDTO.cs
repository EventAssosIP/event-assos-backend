using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class LoginMemberRequestDTO
    {
    [Required(ErrorMessage = "Email is required !")]
    public required EmailAddress EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is required !")]
    public required PasswordHash Password { get; set; }
    }
}