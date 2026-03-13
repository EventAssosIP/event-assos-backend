using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace EventAssos.Domain.Entities
{
    public class Member
    {
        // Liste des admins autorisés
        private static readonly List<string> AdminPseudos = new()
        {
            "admin1",
        };

        [Required]
        public string Pseudo { get; private set; }

        [Required]
        public EmailAddress EmailAddress { get; private set; }

        [Required]
        public Password Password { get; private set; }

        public DateTime Birthdate { get; private set; }

        public Gender Gender { get; private set; }

        [Required]
        public Role Role { get; private set; }

        public Member(string pseudo, EmailAddress email, Password password, DateTime birthdate)
        {
            Pseudo = pseudo ?? throw new ArgumentNullException("Le pseudo doit ");
            EmailAddress = email;
            Password = password;
            Birthdate = birthdate;
            Gender = Gender.Other;

            // Attribution du rôle
            Role = AdminPseudos.Contains(pseudo) ? Role.Admin : Role.User;
        }
    }
}
