using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEventUpdateNotificationAsync(string toEmail, Event updatedEvent);
        Task SendWaitingListPromotionNotificationAsync(string toEmail, Event eventPromoted);
    }
}

