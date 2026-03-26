using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; private set; }

        public string? Pseudo { get; private set; }

        public EmailAddress? EmailAddress { get; private set; }

        public PasswordHash? Password { get; private set; }

        public DateOnly? Birthdate { get; private set; }

        public Gender? Gender { get; private set; }

        public Role Role { get; private set; }

        public DateTime CreatedAt { get; private set; }
    }
}
