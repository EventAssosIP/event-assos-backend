using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Services.Data
{
    public class MemberService(IMemberRepository _memberRepository) : IMemberService
    {
        public Task<Member> CreateAsync(Member member)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingMember = await _memberRepository.ExistsAsync(id);
            if (!existingMember) throw new KeyNotFoundException("Id not found");
            await _memberRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task<Member?> GetByIdAsync(Guid id)
        {
            return await _memberRepository.GetByIdAsync(id);
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Guid id, Member member)
        {
            await _memberRepository.UpdateAsync(id, member);
        }
    }
}