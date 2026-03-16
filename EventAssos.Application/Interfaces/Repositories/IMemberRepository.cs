using EventAssos.Domain.Entities;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IMemberRepository : IBaseRepository<Member, Guid>
    {
        Task<Member?> GetMemberByEmail(EmailAddress email);
    }
    
}
