namespace EventAssos.Application.DTOs.Responses
{
    public class LoginMemberResponseDTO
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
