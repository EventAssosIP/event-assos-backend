using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class EventRepository(EventAssosContext context) : BaseRepository<Event, Guid>(context), IEventRepository
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

        // Récupérer tous les events par categorie
        public async Task<IEnumerable<Event>> GetByEventCategoryAsync(string categoryName)
        {
            return await _entities
                .Include(e => e.Category)
                .ToListAsync(); ;
        }

        // Nombre d'inscrits actifs (WaitingPosition est NULL)
        public async Task<int> GetParticipantCountAsync(Guid eventId)
        {
            return await context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition == null)
                .CountAsync();
        }

        // Liste des membres inscrits actifs (WaitingPosition est NULL)
        public async Task<IEnumerable<Member>> GetRegisteredMembersAsync(Guid eventId)
        {
            return await context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition == null)
                .Include(r => r.Member)
                .Select(r => r.Member!) // Utilisation du ! pour le null-forgiving si nécessaire
                .ToListAsync();
        }

        // Liste d'attente uniquement
        public async Task<IEnumerable<Member>> GetWaitingListMembersAsync(Guid eventId)
        {
            return await context.Set<Registration>()
                .Where(r => r.EventId == eventId && r.WaitingPosition != null)
                .OrderBy(r => r.WaitingPosition)
                .Include(r => r.Member)
                .Select(r => r.Member!)
                .ToListAsync();
        }

    }
}