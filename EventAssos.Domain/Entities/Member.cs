using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }

        public required string Pseudo { get; set; }

        public required EmailAddress EmailAddress { get; set; }

        public required PasswordHash Password { get; set; }

        public DateOnly Birthdate { get; set; }

        public Gender Gender { get; set; }

        public required Role Role { get; set; } = Role.User;

        public required DateTime CreatedAt { get; set; }


        // Propriété pour EF
        public string EmailAddressValue
        {
            get => EmailAddress.Value;
            private set => EmailAddress = EmailAddress.Create(value);
        }
    }
}