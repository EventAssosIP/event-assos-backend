using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventAssos.Application.DTOs.Requests
{
    internal class AddMemberRequestDTO
    {
        [Required]
        public Guid Id { get; }

        [Required(ErrorMessage = "Le pseudo est requis.")]
        public string? Pseudo { get; private set; }

        [Required]
        public EmailAddress? EmailAddress { get; private set; }

        [Required]
        public Password? Password { get; private set; }

        public DateTime Birthdate { get; private set; }

        public Gender Gender { get; private set; }

        [Required]
        public Role Role { get; private set; }
    }
}


