using EventAssos.Domain.Entities;

namespace EventAssos.Application.Interfaces.Services
{
    public interface IRegistrationService
    {
        Task<Registration?> RegisterAsync(Guid eventId, Guid memberId);
    }
}
