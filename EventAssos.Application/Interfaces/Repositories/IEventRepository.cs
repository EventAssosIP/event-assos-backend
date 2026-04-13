using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IEventRepository : IBaseRepository<Event, Guid>
    {
        // AJOUT : Nécessaire pour la nouvelle logique du Service (suppression par entité)
        Task DeleteAsync(Event entity);

        Task<IEnumerable<Event>> GetAllWithRegistrationsAsync();

        Task<IEnumerable<Event>> GetByEventCategoryAsync(string categoryName);

        Task<Event?> GetByIdWithRegistrationsAsync(Guid eventId);

        Task<int> GetParticipantCountAsync(Guid eventId);

        Task<IEnumerable<Member>> GetRegisteredMembersAsync(Guid eventId);

        Task<IEnumerable<Member>> GetWaitingListMembersAsync(Guid eventId);
    }
}