using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }

        public required string Pseudo { get; set; }

        public required EmailAddress EmailAddress { get; set; }

        public PasswordHash Password { get; set; } = null!;

        public DateOnly Birthdate { get; set; }

        public Gender Gender { get; set; }

        public Role Role { get; set; } = Role.User;

        public required DateTime CreatedAt { get; set; }

        public required DateTime UpdatedAt { get; set; }

        private readonly List<Registration> _registrations = new();
        public IReadOnlyCollection<Registration> Registrations => _registrations;

        // Propriété pour EF
        public string EmailAddressValue
        {
            get => EmailAddress.Value;
            private set => EmailAddress = EmailAddress.Create(value);
        }
    }
}