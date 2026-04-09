using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IEventRepository : IBaseRepository<Event, Guid>
    {
        Task<IEnumerable<Event>> GetAllWithRegistrationsAsync();

        Task<IEnumerable<Event>> GetByEventCategoryAsync(string categoryName);

        Task<Event?> GetByIdWithRegistrationsAsync(Guid eventId);

        Task<int> GetParticipantCountAsync(Guid eventId);

        Task<IEnumerable<Member>> GetRegisteredMembersAsync(Guid eventId);

        Task<IEnumerable<Member>> GetWaitingListMembersAsync(Guid eventId);
    }
}
