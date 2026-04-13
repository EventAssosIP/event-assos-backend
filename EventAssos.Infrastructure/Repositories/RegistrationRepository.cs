using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Domain.Entities;
using EventAssos.Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace EventAssos.Infrastructure.Repositories
{
    public class RegistrationRepository(EventAssosContext context) : BaseRepository<Registration, Guid>(context), IRegistrationRepository
    {
        /// <summary>
        /// Récupère le nombre de participants actifs pour un événement
        /// </summary>
        public async Task<int> GetActiveCountByEventIdAsync(Guid eventId)
        {
            return await context.Registrations
                                 .CountAsync(r => r.EventId == eventId && r.WaitingPosition == 0);
        }

        /// <summary>
        /// Récupère la liste d'attente triée par WaitingPosition
        /// </summary>
        public async Task<List<Registration>> GetWaitingListByEventIdAsync(Guid eventId)
        {
            return await context.Registrations
                                 .Include(r => r.Member)
                                 .Where(r => r.EventId == eventId && r.WaitingPosition > 0)
                                 .OrderBy(r => r.WaitingPosition)
                                 .ToListAsync();
        }

        /// <summary>
        /// Récupère un enregistrement spécifique par EventId et MemberId
        /// </summary>
        public async Task<Registration?> GetByEventAndMemberAsync(Guid eventId, Guid memberId)
        {
            return await context.Registrations
                                 .Include(r => r.Member)
                                 .Include(r => r.Event)
                                 .FirstOrDefaultAsync(r => r.EventId == eventId && r.MemberId == memberId);
        }
    }
}