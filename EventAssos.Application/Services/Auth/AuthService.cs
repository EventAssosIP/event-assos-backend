using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services.Auth;
using EventAssos.Application.Interfaces.Services.Tools;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Application.Services.Auth
{
    public class AuthService(
        IMemberRepository _memberRepository,
        IPasswordHasherService _passwordHasherService,
        IJwtService _jwtService
        ) : IAuthService
    {
        public async Task<LoginMemberResponseDto> Login(LoginMemberRequestDTO credentials)
        {
            if (string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                throw new ArgumentException("Email et mot de passe sont requis");

            var member = await _memberRepository.GetMemberByEmail(credentials.Email);
            if (member == null || !_passwordHasherService.VerifyPassword(credentials.Password, member.Password))
                throw new UnauthorizedAccessException("Email ou mot de passe incorrect");

            return await _jwtService.GenerateToken(member);
        }

        public async Task<Member> Register(RegisterMemberRequestDTO credentials)
        {
            var existingUser = await _memberRepository.GetMemberByEmail(credentials.Email);
            if (existingUser != null)
                throw new InvalidOperationException("L'email est déjà utilisée");

            var hashedPassword = _passwordHasherService.HashPassword(credentials.Password);

            var member = new Member
            {
                Id = Guid.NewGuid(),
                Pseudo = credentials.Pseudo,
                EmailAddress = credentials.Email,
                Password = hashedPassword,
                Role = Role.User,
            };

            return await _memberRepository.AddAsync(member);
        }
    }

}


