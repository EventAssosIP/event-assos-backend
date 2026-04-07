using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class EventRepository : BaseRepository<Event, Guid>, IEventRepository
    {
        // On utilise directement le contexte du BaseRepository s'il est protégé, 
        // sinon on garde l'injection locale.
        private readonly EventAssosContext _context;

        public EventRepository(EventAssosContext context)
            : base(context)
        {
            _context = context;
        }

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

        // CORRECTION : Nombre d'inscrits actifs (WaitingPosition est NULL)
        public async Task<int> GetParticipantCountAsync(Guid eventId)
        {
            return await _context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition == null)
                .CountAsync();
        }

        // CORRECTION : Liste des membres inscrits actifs (WaitingPosition est NULL)
        public async Task<IEnumerable<Member>> GetRegisteredMembersAsync(Guid eventId)
        {
            return await _context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition == null)
                .Include(r => r.Member)
                .Select(r => r.Member!) // Utilisation du ! pour le null-forgiving si nécessaire
                .ToListAsync();
        }

        // OPTIONNEL : Liste d'attente uniquement
        public async Task<IEnumerable<Member>> GetWaitingListMembersAsync(Guid eventId)
        {
            return await _context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition != null)
                .OrderBy(r => r.WaitingPosition)
                .Include(r => r.Member)
                .Select(r => r.Member!)
                .ToListAsync();
        }
    }
}