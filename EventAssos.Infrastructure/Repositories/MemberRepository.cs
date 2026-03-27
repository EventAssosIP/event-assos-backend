using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Domain.ValueObjects;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class MemberRepository(EventAssosContext context) : BaseRepository<Member, Guid>(context), IMemberRepository
    {
        public async Task<Member?> GetMemberByEmail(string email)
        {
            return await _entities.FirstOrDefaultAsync(e => e.EmailAddress.Value == email);
        }
    }    
}

