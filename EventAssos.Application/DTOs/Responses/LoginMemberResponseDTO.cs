namespace EventAssos.Application.DTOs.Responses
{
    public class LoginMemberResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
