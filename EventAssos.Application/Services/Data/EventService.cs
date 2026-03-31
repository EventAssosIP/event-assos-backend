using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

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

        public async Task UpdateAsync(Guid id, Event updatedEvent)
        {
            // Récupère l'événement existant
            var existingEvent = await _eventRepository.GetByIdAsync(id);
            if (existingEvent == null)
                throw new KeyNotFoundException("Event not found");

            // 1 Seuls les événements en statut "Pending" (en attente) peuvent être modifiés
            if (existingEvent.Status != EventStatus.Pending)
                throw new InvalidOperationException("Only events in 'Pending' status can be updated");

            // 2 Vérifie que le nouveau MaxParticipants ne descend pas en dessous des participants actuels
            int currentParticipants = await _eventRepository.GetParticipantCountAsync(id);
            if (updatedEvent.MaxParticipants < currentParticipants)
                throw new InvalidOperationException("Cannot set MaxParticipants below current registered members");

            // 3 Détecte les changements d'informations clés pour envoyer un email
            bool keyInfoChanged =
                existingEvent.StartDate != updatedEvent.StartDate ||
                existingEvent.EndDate != updatedEvent.EndDate ||
                existingEvent.Location != updatedEvent.Location;

            // 4 Mise à jour de l'événement
            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.StartDate = updatedEvent.StartDate;
            existingEvent.EndDate = updatedEvent.EndDate;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.MinParticipants = updatedEvent.MinParticipants;
            existingEvent.MaxParticipants = updatedEvent.MaxParticipants;
            existingEvent.WaitingListActive = updatedEvent.WaitingListActive;
            existingEvent.RegistrationDeadline = updatedEvent.RegistrationDeadline;
            existingEvent.UpdatedAt = DateTime.UtcNow;

            await _eventRepository.UpdateAsync(id, existingEvent);

            // 5 Envoi d'e-mail si infos clés modifiées
            if (keyInfoChanged)
            {
                var registeredMembers = await _eventRepository.GetRegisteredMembersAsync(id);
                foreach (var member in registeredMembers)
                {
                    await _emailService.SendEventUpdateNotificationAsync(member.EmailAddress, existingEvent);
                }
            }
        }
    }
}
