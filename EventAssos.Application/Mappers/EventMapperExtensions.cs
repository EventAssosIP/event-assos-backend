using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Domain.Entities;

namespace EventAssos.Application.Mappers
{
    public static class EventMapperExtension
    {
        public static Event ToEvent(this AddEventRequestDTO request)
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                MinParticipants = request.MinParticipants,
                MaxParticipants = request.MaxParticipants,
                WaitingListActive = request.WaitingListActive,
                RegistrationDeadline = request.RegistrationDeadline,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static EventResponseDTO ToEventResponseDTO(this Event entity)
        {
            return new EventResponseDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                MinParticipants = entity.MinParticipants,
                MaxParticipants = entity.MaxParticipants,
                Status = entity.Status.ToString(),
                WaitingListActive = entity.WaitingListActive,
                RegistrationDeadline = entity.RegistrationDeadline,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
            public static IEnumerable<EventResponseDTO> ToEventResponseDTOs(this IEnumerable<Event> events)
        {
            return events.Select(e => e.ToEventResponseDTO());
        }
    }
}