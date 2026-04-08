using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IMemberRepository : IBaseRepository<Member, Guid>
    {
        Task<Member?> GetMemberByEmail(string EmailAddress);
    }
    
}
