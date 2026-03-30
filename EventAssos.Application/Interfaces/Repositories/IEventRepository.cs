using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Repositories
{
    public interface IEventRepository : IBaseRepository<Event, Guid>
    {
    }
}
