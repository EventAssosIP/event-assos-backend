using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }

        [Required]
        public required string Pseudo { get; set; } = null!;

        [Required]
        public required EmailAddress EmailAddress { get; set; }

        [Required]
        public required PasswordHash Password { get; set; }

        public DateOnly Birthdate { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public required Role Role { get; set; }
    }
}
