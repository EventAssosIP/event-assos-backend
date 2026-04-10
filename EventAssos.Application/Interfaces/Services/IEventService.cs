using EventAssos.Application.DTOs.Requests;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Services
{
    public interface IEventService : IBaseService<Event, Guid, UpdateEventRequestDTO>
    {
    }
}