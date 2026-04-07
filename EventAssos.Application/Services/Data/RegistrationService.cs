using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;

namespace EventAssos.Application.Services.Data
{
    public class RegistrationService(
        IRegistrationRepository _registrationRepository,
        IEventRepository _eventRepository,
        IUnitOfWork _unitOfWork)
        : IRegistrationService
    {
        public async Task<Registration?> RegisterAsync(Guid eventId, Guid memberId)
        {
            var eventEntity = await _eventRepository.GetByIdWithRegistrationsAsync(eventId);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            // --- VALIDATION DES RÈGLES MÉTIER ---

            // 1. Vérification du statut
            if (eventEntity.Status != EventStatus.InProgress)
                throw new InvalidOperationException("Registration is not allowed for this event status.");

            // 2. Vérification de la date limite
            if (DateTime.UtcNow > eventEntity.RegistrationDeadline)
                throw new InvalidOperationException("The registration deadline has passed.");

            // 3. Vérification de double inscription
            if (eventEntity.Registrations.Any(r => r.MemberId == memberId))
                throw new InvalidOperationException("Member is already registered for this event.");

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
                    WaitingPosition = null // Inscription directe
                };
            }
            else
            {
                // Règle 4 : La liste d'attente doit être active
                if (!eventEntity.IsWaitingListActive)
                    throw new InvalidOperationException("The event is full and the waiting list is disabled.");

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

            // VALIDATION ATOMIQUE (ACID)
            await _unitOfWork.SaveChangesAsync();

            return registration;
        }

        public async Task UnregisterAsync(Guid eventId, Guid memberId)
        {
            var eventEntity = await _eventRepository.GetByIdWithRegistrationsAsync(eventId);
            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            var registration = eventEntity.Registrations.FirstOrDefault(r => r.MemberId == memberId);
            if (registration == null)
                throw new KeyNotFoundException("Registration not found for this member.");

            int? removedPosition = registration.WaitingPosition;
            bool wasActiveMember = (removedPosition == null);

            // 1. Marquage pour suppression (dans le ChangeTracker d'EF)
            await _registrationRepository.DeleteAsync(registration.Id);

            if (wasActiveMember)
            {
                // 2. PROMOTION : Le premier de la liste d'attente devient actif
                var nextInLine = eventEntity.Registrations
                    .Where(r => r.WaitingPosition != null)
                    .OrderBy(r => r.WaitingPosition)
                    .FirstOrDefault();

                if (nextInLine != null)
                {
                    nextInLine.WaitingPosition = null;

                    // 3. RÉ-INDEXATION : On remonte le reste de la liste d'attente
                    var remainingWaiting = eventEntity.Registrations
                        .Where(r => r.WaitingPosition != null && r.Id != nextInLine.Id)
                        .OrderBy(r => r.WaitingPosition)
                        .ToList();

                    int newPos = 1;
                    foreach (var r in remainingWaiting) r.WaitingPosition = newPos++;
                }
            }
            else
            {
                // 4. RÉORGANISATION : Ajustement pour ceux qui étaient derrière le membre supprimé
                var peopleBehind = eventEntity.Registrations
                    .Where(r => r.WaitingPosition > removedPosition)
                    .OrderBy(r => r.WaitingPosition)
                    .ToList();

                foreach (var r in peopleBehind) r.WaitingPosition--;
            }

            // VALIDATION ATOMIQUE : Persiste tous les changements ou aucun
            await _unitOfWork.SaveChangesAsync();
        }
    }
}