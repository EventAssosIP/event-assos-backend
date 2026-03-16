using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Domain.Entities
{
    public class Member
    {
        public Guid Id { get; set; }

        [Required]
        public string? Pseudo { get; set; } = null!;

        [Required]
        public EmailAddress EmailAddress { get; set; }

        [Required]
        public Password Password { get; set; }

        public DateTime Birthdate { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public Role Role { get; set; }
    }
}
