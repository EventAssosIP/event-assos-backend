using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
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
            var exists = await _eventRepository.ExistsAsync(id);
            if (!exists) throw new KeyNotFoundException("Event not found");

            await _eventRepository.DeleteAsync(id);
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
        // UPDATE
        // ==========================
        public async Task UpdateAsync(Guid id, Event updatedEvent)
        {
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent == null)
                throw new KeyNotFoundException("Event not found");

            if (existingEvent.Status != EventStatus.Pending)
                throw new InvalidOperationException("Only events in 'Pending' status can be updated");

            int currentParticipants = await _eventRepository.GetParticipantCountAsync(id);
            if (updatedEvent.MaxParticipants < currentParticipants)
                throw new InvalidOperationException("Cannot set MaxParticipants below current registered members");

            bool keyInfoChanged =
                existingEvent.StartDate != updatedEvent.StartDate ||
                existingEvent.EndDate != updatedEvent.EndDate ||
                existingEvent.Location != updatedEvent.Location;

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.StartDate = updatedEvent.StartDate;
            existingEvent.EndDate = updatedEvent.EndDate;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.MinParticipants = updatedEvent.MinParticipants;
            existingEvent.MaxParticipants = updatedEvent.MaxParticipants;
            existingEvent.IsWaitingListActive = updatedEvent.IsWaitingListActive;
            existingEvent.RegistrationDeadline = updatedEvent.RegistrationDeadline;
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