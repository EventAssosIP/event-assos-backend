namespace EventAssos.Application.Interfaces.Services.Tools
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
