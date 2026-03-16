namespace EventAssos.Application.Interfaces.Services.Tools
{ 
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedPassword);
    }
}
