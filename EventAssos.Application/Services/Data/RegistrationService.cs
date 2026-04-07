using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Application.Services.Data
{
    public class RegistrationService(IRegistrationRepository _registrationRepository, IEventRepository _eventRepository)
        : IRegistrationService
    {
        public async Task<Registration?> RegisterAsync(Guid eventId, Guid memberId)
        {
            var eventEntity = await _eventRepository.GetByIdWithRegistrationsAsync(eventId);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found");

            // --- VÉRIFICATIONS RÈGLES MÉTIER ---

            // 1. Statut « en attente »
            if (eventEntity.Status.ToString() != EventStatus.InProgress.ToString())
                throw new InvalidOperationException("L'événement n'est pas ouvert aux inscriptions.");

            // 2. Date limite non dépassée
            if (DateTime.UtcNow > eventEntity.RegistrationDeadline)
                throw new InvalidOperationException("La date limite d'inscription est dépassée.");

            // 3. Pas de double inscription
            if (eventEntity.Registrations.Any(r => r.MemberId == memberId))
                throw new InvalidOperationException("Member already registered");

            // --- LOGIQUE D'INSCRIPTION ---

            int activeCount = eventEntity.Registrations.Count(r => r.WaitingPosition == null);
            Registration registration;

            if (activeCount < eventEntity.MaxParticipants)
            {
                registration = new Registration
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    MemberId = memberId,
                    RegisteredAt = DateTime.UtcNow,
                    WaitingPosition = null // Inscrit direct
                };
            }
            else
            {
                // Règle 4 : La liste d'attente doit être active
                // (Si tu n'as pas ce booléen, retire cette condition)
                if (!eventEntity.IsWaitingListActive)
                    throw new InvalidOperationException("L'événement est complet.");

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

            // On stocke les infos avant suppression
            int? removedPosition = registration.WaitingPosition;
            bool wasActive = (removedPosition == null);

            // 1. Supprimer l'inscription
            await _registrationRepository.DeleteAsync(registration.Id);

            if (wasActive)
            {
                // 2. PROMOTION : Si un membre actif part, le 1er de la liste d'attente monte
                var nextInLine = eventEntity.Registrations
                    .Where(r => r.WaitingPosition != null)
                    .OrderBy(r => r.WaitingPosition)
                    .FirstOrDefault();

                if (nextInLine != null)
                {
                    nextInLine.WaitingPosition = null;
                    await _registrationRepository.UpdateAsync(nextInLine);

                    // 3. RECALCUL : On fait remonter tous les autres
                    var remainingWaiting = eventEntity.Registrations
                        .Where(r => r.WaitingPosition != null && r.Id != nextInLine.Id)
                        .OrderBy(r => r.WaitingPosition)
                        .ToList();

                    int newPos = 1;
                    foreach (var r in remainingWaiting)
                    {
                        r.WaitingPosition = newPos++;
                        await _registrationRepository.UpdateAsync(r);
                    }
                }
            }
            else
            {
                // 4. RÉORGANISATION : Si c'est quelqu'un en liste d'attente qui part,
                // on décale seulement ceux qui étaient derrière lui.
                var peopleBehind = eventEntity.Registrations
                    .Where(r => r.WaitingPosition > removedPosition)
                    .OrderBy(r => r.WaitingPosition)
                    .ToList();

                foreach (var r in peopleBehind)
                {
                    r.WaitingPosition--;
                    await _registrationRepository.UpdateAsync(r);
                }
            }
        }
    }
}