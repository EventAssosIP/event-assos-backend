
using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Services.Data
{
    internal class MemberService
    {
    }
}

public class UserService(IMemberRepository _memberRespository) : IMemberService
{
    public async Task<Member> CreateAsync(Member member)
    {
        // Member
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        var existingMember = await _memberRespository.ExistsAsync(id);
        if (!existingMember) throw new KeyNotFoundException("Id not found");
        await _memberRespository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _memberRespository.GetAllAsync();
    }

    public async Task<Member?> GetByIdAsync(Guid id)
    {
        return await _memberRespository.GetByIdAsync(id);
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Guid id, Member user)
    {
        await _memberRespository.UpdateAsync(id, user);
    }
}