using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Application.DTOs.Requests;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Application.Services.Data
{
    public class EventService(IEventRepository _eventRepository, IEmailService _emailService) : IEventService
    {
        // ==========================
        // CREATE
        // ==========================
        public async Task<Event> CreateAsync(Event newEvent)
        {
            newEvent.Id = Guid.NewGuid();
            newEvent.CreatedAt = DateTime.UtcNow;
            newEvent.UpdatedAt = DateTime.UtcNow;

            return await _eventRepository.AddAsync(newEvent);
        }

        // ==========================
        // DELETE
        // ==========================
        public async Task DeleteAsync(Guid id)
        {
            // Plus efficace : On récupère l'entité directement
            var eventToDelete = await _eventRepository.GetByIdAsync(id);
            if (eventToDelete == null) throw new KeyNotFoundException("EVENT_NOT_FOUND");

            await _eventRepository.DeleteAsync(eventToDelete);
        }

        // ==========================
        // GET ALL
        // ==========================
        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _eventRepository.GetAllAsync();
        }

        // ==========================
        // GET BY ID
        // ==========================
        public async Task<Event?> GetByIdAsync(Guid id)
        {
            return await _eventRepository.GetByIdAsync(id);
        }

        // ==========================
        // UPDATE (Version Partielle avec DTO)
        // ==========================
        public async Task UpdateAsync(Guid id, UpdateEventRequestDTO dto)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent == null)
                throw new KeyNotFoundException("EVENT_NOT_FOUND");

            if (existingEvent.Status != EventStatus.Pending)
                throw new InvalidOperationException("ONLY_PENDING_CAN_BE_UPDATED");

            // Validation : On ne vérifie le nombre de participants que si MaxParticipants est fourni
            if (dto.MaxParticipants.HasValue)
            {
                int currentParticipants = await _eventRepository.GetParticipantCountAsync(id);
                if (dto.MaxParticipants.Value < currentParticipants)
                    throw new InvalidOperationException("MAX_PARTICIPANTS_TOO_LOW");
            }

            // Détection des changements critiques (uniquement si les nouvelles valeurs sont fournies)
            bool keyInfoChanged =
                (dto.StartDate.HasValue && dto.StartDate != existingEvent.StartDate) ||
                (dto.EndDate.HasValue && dto.EndDate != existingEvent.EndDate) ||
                (dto.Location != null && dto.Location != existingEvent.Location);

            // Mapping Partiel : On ne remplace que ce qui n'est pas NULL dans le DTO
            if (!string.IsNullOrWhiteSpace(dto.Name)) existingEvent.Name = dto.Name;
            if (dto.Description != null) existingEvent.Description = dto.Description;
            if (dto.StartDate.HasValue) existingEvent.StartDate = dto.StartDate.Value;
            if (dto.EndDate.HasValue) existingEvent.EndDate = dto.EndDate.Value;
            if (dto.Location != null) existingEvent.Location = dto.Location;
            if (dto.MinParticipants.HasValue) existingEvent.MinParticipants = dto.MinParticipants.Value;
            if (dto.MaxParticipants.HasValue) existingEvent.MaxParticipants = dto.MaxParticipants.Value;
            if (dto.WaitingListActive.HasValue) existingEvent.IsWaitingListActive = dto.WaitingListActive.Value;
            if (dto.RegistrationDeadline.HasValue) existingEvent.RegistrationDeadline = dto.RegistrationDeadline.Value;
            if (dto.CategoryId.HasValue) existingEvent.CategoryId = dto.CategoryId.Value;

            existingEvent.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(existingEvent);

            if (keyInfoChanged)
            {
                var registeredMembers = await _eventRepository.GetRegisteredMembersAsync(id);
                foreach (var member in registeredMembers)
                {
                    await _emailService.SendEventUpdateNotificationAsync(member.EmailAddress.ToString(), existingEvent);
                }
            }
        }
    }
}