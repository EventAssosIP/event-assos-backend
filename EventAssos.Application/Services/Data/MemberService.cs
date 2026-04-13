using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.Interfaces.Repositories;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Application.Interfaces.Services.Tools;
using EventAssos.Application.Services.Tools;
using EventAssos.Domain.Entities;
using EventAssos.Domain.Enums;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.Application.Services.Data
{
    public class MemberService(IMemberRepository _memberRepository, IPasswordHasherService _passwordHasher) : IMemberService
    {
        // ==========================
        // CREATE
        // ==========================
        public async Task<Member> CreateAsync(Member member)
        {
            // 1. Vérifier si le membre a déjà un PasswordHash (cas du self-register via DTO)
            // Sinon, on en génère un automatiquement (cas Admin)
            if (member.Password == null || string.IsNullOrEmpty(member.Password.Value))
            {
                // Génération du mot de passe clair respectant ton Regex
                string generatedRawPassword = PasswordGenerator.Generate(12);

                // Validation via ton VO Password (Regex)
                var validatedPassword = Password.Create(generatedRawPassword);

                // Hashage via Argon2
                string hash = _passwordHasher.HashPassword(validatedPassword.Value);

                // Stockage dans l'entité
                member.Password = new PasswordHash(hash);

                // TODO : Envoyer le 'generatedRawPassword' par mail ici
            }

            member.Id = Guid.NewGuid();
            member.CreatedAt = DateTime.UtcNow;
            member.UpdatedAt = DateTime.UtcNow;

            return await _memberRepository.AddAsync(member);
        }

        // ==========================
        // DELETE
        // ==========================
        public async Task DeleteAsync(Guid id)
        {
            // On récupère l'entité directement pour éviter le double appel Exists/Delete
            var memberToDelete = await _memberRepository.GetByIdAsync(id);
            if (memberToDelete == null) throw new KeyNotFoundException("MEMBER_NOT_FOUND");

            await _memberRepository.DeleteAsync(memberToDelete);
        }

        // ==========================
        // GET ALL
        // ==========================
        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        // ==========================
        // GET BY ID
        // ==========================
        public async Task<Member?> GetByIdAsync(Guid id)
        {
            return await _memberRepository.GetByIdAsync(id);
        }

        // ==========================
        // UPDATE (Version Partielle)
        // ==========================
        public async Task UpdateAsync(Guid id, UpdateMemberRequestDTO dto)
        {
            var existingMember = await _memberRepository.GetByIdAsync(id);
            if (existingMember == null) throw new KeyNotFoundException("MEMBER_NOT_FOUND");

            // Mapping partiel : On ne met à jour que ce qui est fourni
            if (!string.IsNullOrWhiteSpace(dto.Pseudo)) existingMember.Pseudo = dto.Pseudo;
            if (!string.IsNullOrWhiteSpace(dto.EmailAddress)) existingMember.EmailAddress = EmailAddress.Create(dto.EmailAddress);

            // Password Update Logic
            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // ÉTAPE 1 : Valider les règles métier (Regex) via le VO Password
                // Si le mot de passe est invalide, le .Create() lèvera l'ArgumentException ici.
                var validatedPassword = Password.Create(dto.Password);

                // ÉTAPE 2 : Hasher la valeur validée
                string hashedPassword = _passwordHasher.HashPassword(validatedPassword.Value);

                // ÉTAPE 3 : Créer le VO de stockage et l'assigner à l'entité
                // Note: Assure-toi que la propriété s'appelle bien PasswordHash dans ton entité Member
                existingMember.Password = new PasswordHash(hashedPassword);
            }

            if (!string.IsNullOrWhiteSpace(dto.Birthdate))
            {
                var validBirthDate = DateOnly.Parse(dto.Birthdate);
                existingMember.Birthdate = validBirthDate;
            }

            if (!string.IsNullOrWhiteSpace(dto.Gender))
            {
                if (Enum.TryParse<Gender>(dto.Gender, true, out var validGender))
                {
                    existingMember.Gender = validGender;
                }
                else
                {
                    throw new ArgumentException("INVALID_GENDER_VALUE");
                }
            }

            existingMember.UpdatedAt = DateTime.UtcNow;
            await _memberRepository.UpdateAsync(existingMember);

            // Si tu as des types nullables dans ton DTO (ex: Date de naissance, etc.)
            // if (dto.BirthDate.HasValue) existingMember.BirthDate = dto.BirthDate.Value;

            existingMember.UpdatedAt = DateTime.UtcNow;

            await _memberRepository.UpdateAsync(existingMember);
        }
    }
}