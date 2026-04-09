using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Services.Data
{
    internal class EventCategoryService(IEventCategoryRepository _eventCategoryRepository) : IEventCategoryService
    {
        // ==========================
        // CREATE
        // ==========================
        public async Task<EventCategory> CreateAsync(EventCategory newEventCategory)
        {
            if (string.IsNullOrWhiteSpace(newEventCategory.Name) || newEventCategory.Name.Trim().Length < 2)
            {
                throw new ArgumentException("Name must contain at least 2 characters");
            }

            newEventCategory.Id = Guid.NewGuid();
            newEventCategory.Name = Capitalize(newEventCategory.Name);

            return await _eventCategoryRepository.AddAsync(newEventCategory);
        }

        // ==========================
        // DELETE
        // ==========================
        public async Task DeleteAsync(Guid id)
        {
            var exists = await _eventCategoryRepository.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Event category not found");

            await _eventCategoryRepository.DeleteAsync(id);
        }

        // ==========================
        // GET ALL
        // ==========================
        public async Task<IEnumerable<EventCategory>> GetAllAsync()
        {
            return await _eventCategoryRepository.GetAllAsync();
        }

        // ==========================
        // GET BY ID
        // ==========================
        public async Task<EventCategory?> GetByIdAsync(Guid id)
        {
            return await _eventCategoryRepository.GetByIdAsync(id);
        }

        // ==========================
        // UPDATE
        // ==========================
        public async Task UpdateAsync(Guid id, EventCategory entity)
        {
            var existingEventCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (existingEventCategory == null)
                throw new KeyNotFoundException("Event category not found");

            if (!string.IsNullOrWhiteSpace(entity.Name))
            {
                var name = entity.Name.Trim().ToLower();
                existingEventCategory.Name = Capitalize(name);
            }

            await _eventCategoryRepository.UpdateAsync(existingEventCategory);
        }

        // ==========================
        // METHODE TO CAPITALIZE
        // ==========================
        private string Capitalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var text = input.Trim().ToLower();
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
