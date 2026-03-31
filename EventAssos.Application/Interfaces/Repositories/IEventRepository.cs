using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IEventRepository : IBaseRepository<Event, Guid>
    {
        Task<IEnumerable<Event>> GetAllWithRegistrationsAsync();

        Task<Event?> GetByIdWithRegistrationsAsync(Guid eventId);
    }
}
