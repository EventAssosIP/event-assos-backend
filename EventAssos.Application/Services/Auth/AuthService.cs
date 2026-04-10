using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services.Auth;
using EventAssos.Application.Interfaces.Services.Tools;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Application.Services.Auth
{
    public class AuthService(
        IMemberRepository _memberRepository,
        IPasswordHasherService _passwordHasherService,
        IJwtService _jwtService
        ) : IAuthService
    {
        public async Task<LoginMemberResponseDTO> Login(LoginMemberRequestDTO credentials)
        {         
            // Récupère le membre
            var member = await _memberRepository.GetMemberByEmail(credentials.EmailAddress);
            if (member == null || !_passwordHasherService.VerifyPassword(credentials.Password, member.Password.Value))
                throw new UnauthorizedAccessException("Invalid email or password");

            return await _jwtService.GenerateToken(member);
        }

        public async Task<Member> Register(RegisterMemberRequestDTO credentials)
        {
            // Vérifie si email déjà utilisé
            var existingMember = await _memberRepository.GetMemberByEmail(credentials.Email);
            if (existingMember != null)
                throw new InvalidOperationException("Email alrady used");

            // Hash le mot de passe
            var hashedPassword = _passwordHasherService.HashPassword(credentials.Password);

            var member = new Member
            {
                Id = Guid.NewGuid(),
                Pseudo = credentials.Pseudo,
                EmailAddress = EmailAddress.Create(credentials.Email),
                Password = new PasswordHash(hashedPassword),
                Role = Role.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return await _memberRepository.AddAsync(member);
        }
    }

}


