using EventAssos.Application.DTOs.Responses;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Services.Auth
{
    public interface IJwtService
    {
        Task<LoginMemberResponseDto> GenerateToken(Member member);
    }
}
