using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Services.Data
{
    public class EventCategoryService(IEventCategoryRepository _eventCategoryRepository) : IEventCategoryService
    {
        // ==========================
        // CREATE
        // ==========================
        public async Task<EventCategory> CreateAsync(EventCategory entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name) || entity.Name.Trim().Length < 2)
            {
                throw new ArgumentException("NAME_TOO_SHORT");
            }

            entity.Id = Guid.NewGuid();
            entity.Name = Capitalize(entity.Name);

            return await _eventCategoryRepository.AddAsync(entity);
        }

        // ==========================
        // DELETE
        // ==========================
        public async Task DeleteAsync(Guid id)
        {
            // On récupère l'entité pour être cohérent avec la nouvelle approche
            var category = await _eventCategoryRepository.GetByIdAsync(id);
            if (category == null) throw new KeyNotFoundException("CATEGORY_NOT_FOUND");

            await _eventCategoryRepository.DeleteAsync(category);
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
        public async Task UpdateAsync(Guid id, UpdateEventCategoryRequestDTO dto)
        {
            var existingCategory = await _eventCategoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                throw new KeyNotFoundException("CATEGORY_NOT_FOUND");

            // On ne met à jour que si le nom est fourni et non vide
            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                existingCategory.Name = Capitalize(dto.Name);
            }

            await _eventCategoryRepository.UpdateAsync(existingCategory);
        }

        // ==========================
        // HELPER
        // ==========================
        private string Capitalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var text = input.Trim().ToLower();
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}