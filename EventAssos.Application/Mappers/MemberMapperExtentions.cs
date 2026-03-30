using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Domain.Entities;
using EventAssos.Domain.ValueObjects;
using EventAssos.Domain.Enums;

namespace EventAssos.Application.Mappers
{
    public static class MemberMapperExtentions
    {
        public static Member ToMember(this AddMemberRequestDTO request)
        {
            return new Member
            {
                Id = Guid.NewGuid(),
                Pseudo = request.Pseudo,
                EmailAddress = EmailAddress.Create(request.EmailAddress),
                Password = new PasswordHash(request.Password),
                Role = Role.User,
                CreatedAt = DateTime.UtcNow,
            };
        }

        public static MemberResponseDTO ToMemberResponseDTO(this Member member)
        {
            return new MemberResponseDTO
            {
                Id = member.Id,
                Pseudo = member.Pseudo,
                EmailAddress = member.EmailAddress.ToString(),
                CreatedAt = member.CreatedAt,
            };
        }

        public static IEnumerable<MemberResponseDTO> ToMemberResponseDTOs(this IEnumerable<Member> members)
        {
            return members.Select(m => m.ToMemberResponseDTO()).ToList();
        }
    }
}