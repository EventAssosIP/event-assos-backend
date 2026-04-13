using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class EventCategoryRepository(EventAssosContext context) : BaseRepository<EventCategory, Guid>(context), IEventCategoryRepository
    {
        public async Task<EventCategory?> GetByNameAsync(string name)
        {
            return await _entities
                .Include(e => e.Name)
                .FirstOrDefaultAsync(e => e.Name == name);
        }
    }
}
