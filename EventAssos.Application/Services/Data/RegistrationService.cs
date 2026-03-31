using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Services.Data
{
    public class RegistrationService(IRegistrationRepository _registrationRepository, IEventRepository _eventRepository)
        : IRegistrationService
    {
        public async Task<Registration?> RegisterAsync(Guid eventId, Guid memberId)
        {
            // 1 récupérer l’event avec ses registrations
            var eventEntity = await _eventRepository.GetByIdWithRegistrationsAsync(eventId);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found");

            // 2 vérifier double inscription
            bool alreadyRegistered = eventEntity.Registrations
                .Any(r => r.MemberId == memberId);

            if (alreadyRegistered)
                throw new InvalidOperationException("Member already registered");

            // 3 compter les participants actifs
            int activeCount = eventEntity.Registrations
                .Count(r => r.WaitingPosition == null);

            Registration registration;

            // 4 place dispo ?
            if (activeCount < eventEntity.MaxParticipants)
            {
                registration = new Registration
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    MemberId = memberId,
                    RegisteredAt = DateTime.UtcNow,
                    WaitingPosition = null // inscrit direct
                };
            }
            else
            {
                // 5 waiting list
                int lastPosition = eventEntity.Registrations
                    .Where(r => r.WaitingPosition != null)
                    .Select(r => r.WaitingPosition!.Value)
                    .DefaultIfEmpty(0)
                    .Max();

                registration = new Registration
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    MemberId = memberId,
                    RegisteredAt = DateTime.UtcNow,
                    WaitingPosition = lastPosition + 1
                };
            }

            await _registrationRepository.AddAsync(registration);

            return registration;
        }

        public async Task UnregisterAsync(Guid eventId, Guid memberId)
        {
            var eventEntity = await _eventRepository.GetByIdWithRegistrationsAsync(eventId);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found");

            var registration = eventEntity.Registrations
                .FirstOrDefault(r => r.MemberId == memberId);

            if (registration == null)
                throw new KeyNotFoundException("Registration not found");

            bool wasActive = registration.WaitingPosition == null;

            await _registrationRepository.DeleteAsync(registration.Id);

            // promotion auto
            if (wasActive)
            {
                var next = eventEntity.Registrations
                    .Where(r => r.WaitingPosition != null)
                    .OrderBy(r => r.WaitingPosition)
                    .FirstOrDefault();

                if (next != null)
                {
                    next.WaitingPosition = null;

                    // recalcul des positions
                    var waitingList = eventEntity.Registrations
                        .Where(r => r.WaitingPosition != null)
                        .OrderBy(r => r.WaitingPosition)
                        .ToList();

                    int position = 1;
                    foreach (var r in waitingList)
                    {
                        r.WaitingPosition = position++;
                    }
                }
            }
        }
    }
}