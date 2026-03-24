using EventAssos.Application.DTOs.Responses;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Mappers
{
    public static class MemberMapperExtentions
    {
        public static MemberResponseDTO ToMemberResponseDTO(this Member member)
        {
            return new MemberResponseDTO
            {
                Id = member.Id,
                Pseudo = member.Pseudo,
                EmailAddress = member.EmailAddress,
            };
        }

        public static IEnumerable<MemberResponseDTO> ToMemberResponseDTOs(this IEnumerable<Member> members)
        {
            return members.Select(u => u.ToMemberResponseDTO()).ToList();
        }
    }
}