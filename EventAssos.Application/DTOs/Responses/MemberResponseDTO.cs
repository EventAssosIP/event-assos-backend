using EventAssos.Domain.ValueObjects;

namespace EventAssos.Application.DTOs.Responses
{
    public class MemberResponseDTO
    {
    public Guid Id { get; set; }
    public string? Pseudo { get; set; }
    public EmailAddress? EmailAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    }
}
