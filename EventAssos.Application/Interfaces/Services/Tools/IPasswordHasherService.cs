using EventAssos.Domain.ValueObjects;

namespace EventAssos.Application.Interfaces.Services.Tools
{ 
    public interface IPasswordHasherService
    {
        string HashPassword(PasswordHash password);
        bool VerifyPassword(PasswordHash password, PasswordHash storedPassword);
    }
}
