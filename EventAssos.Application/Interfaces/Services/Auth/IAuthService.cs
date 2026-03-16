using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Services.Auth
{
    public interface IAuthService
    {
        Task<Member> Register(RegisterMemberRequestDTO credentials);
        Task<LoginMemberResponseDto> Login(LoginMemberRequestDTO credentials);
    } 
}

