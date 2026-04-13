namespace EventAssos.Domain.Entities
{
    public class EventCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Propriété de navigation
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
