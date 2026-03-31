using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class EventRepository(EventAssosContext context)
        : BaseRepository<Event, Guid>(context), IEventRepository
    {

        // Récupère un Event avec toutes ses inscriptions et les membres associés
        public async Task<Event?> GetByIdWithRegistrationsAsync(Guid eventId)
        {
            return await _entities
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Member)
                .FirstOrDefaultAsync(e => e.Id == eventId);
        }

        // Récupérer tous les events avec leurs inscriptions
        public async Task<IEnumerable<Event>> GetAllWithRegistrationsAsync()
        {
            return await _entities
                .Include(e => e.Registrations)
                    .ThenInclude(r => r.Member)
                .ToListAsync();
        }

        // Nombre d'inscrits actifs pour un event
        public async Task<int> GetParticipantCountAsync(Guid eventId)
        {
            return await _context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition == 0)
                .CountAsync();
        }

        // Liste des membres inscrits (pour notifications)
        public async Task<IEnumerable<Member>> GetRegisteredMembersAsync(Guid eventId)
        {
            return await _context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition == 0)
                .Include(r => r.Member)
                .Select(r => r.Member)
                .ToListAsync();
        }
    }
}