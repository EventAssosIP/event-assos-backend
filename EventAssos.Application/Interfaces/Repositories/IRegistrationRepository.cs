using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IRegistrationRepository : IBaseRepository<Registration, Guid>
    {
        Task<int> GetActiveCountByEventIdAsync(Guid eventId);

        Task<List<Registration>> GetWaitingListByEventIdAsync(Guid eventId);

        Task<Registration?> GetByEventAndMemberAsync(Guid eventId, Guid memberId);
    }
}

