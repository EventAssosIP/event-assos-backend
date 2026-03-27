using System.ComponentModel.DataAnnotations;

namespace EventAssos.Application.DTOs.Requests
{
    public class LoginMemberRequestDTO
    {
    [Required(ErrorMessage = "Email is required !")]
    public required string EmailAddress { get; set; }

    [Required(ErrorMessage = "Password is required !")]
    public required string Password { get; set; }
    }
}