using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Services.Data
{
    public class EventService(IEventRepository _eventRepository) : IEventService
    {
        public Task<Event> CreateAsync(Event _event)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var existingEvent = await _eventRepository.ExistsAsync(id);
            if (!existingEvent) throw new KeyNotFoundException("Id not found");
            await _eventRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await _eventRepository.GetByIdAsync(id);
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Guid id, Event _event)
        {
            await _eventRepository.UpdateAsync(id, _event);
        }
    }
}
