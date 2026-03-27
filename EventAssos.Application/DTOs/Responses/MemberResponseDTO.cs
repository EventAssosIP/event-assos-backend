using EventAssos.Domain.ValueObjects;

namespace EventAssos.Application.DTOs.Responses
{
    public class MemberResponseDTO
    {
    public Guid Id { get; set; }
    public string Pseudo { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    }
}
