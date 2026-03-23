using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Domain.ValueObjects;
using EventAssos.Infrastructure.DataBase.Context;

namespace EventAssos.Infrastructure.Repositories
{
    public class UserRepository(EventAssosContext context) : BaseRepository<Member, Guid>(context), IMemberRepository
    {
        public async Task<Member?> GetMemberByEmail(EmailAddress email)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Email == email);
        }
    }    
}

