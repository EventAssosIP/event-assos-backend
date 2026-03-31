using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class EventRepository(EventAssosContext context) : BaseRepository<Event, Guid>(context), IEventRepository
    {
        public async Task<IEnumerable<Event>> GetEventsByStatusAndDate(EventStatus status, DateTime fromDate)
        {
            return await _entities
                .Where(e => e.Status == status && e.CreatedAt >= fromDate)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();
        }
    }
}

