using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IEventCategoryRepository : IBaseRepository<EventCategory, Guid>
    {
        Task<EventCategory?> GetByNameAsync(string name);
    }
}
