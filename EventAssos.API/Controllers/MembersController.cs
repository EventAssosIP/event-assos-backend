using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.Interfaces.Services;
using EventAssos.Application.Mappers;
using EventAssos.Domain.Entities;
using EventAssos.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventAssos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Il faut être connecté pour accéder à ces routes
    public class MembersController(IMemberService _memberService) : ControllerBase
    {
        // ==========================================
        // CREATE BY ADMIN
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> CreateByAdmin(AddMemberRequestDTO dto)
        {
            var newMember = new Member
            {
                Pseudo = dto.Pseudo,
                EmailAddress = EmailAddress.Create(dto.EmailAddress),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            // Le MemberService détectera le NULL et générera le mot de passe auto
            var result = await _memberService.CreateAsync(newMember);
            return CreatedAtAction(nameof(GetMember), new { id = result.Id }, result);
        }

        // ==========================================
        // SEUL L'ADMIN : Liste tous les membres
        // ==========================================
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetMembers()
        {
            var members = await _memberService.GetAllAsync();
            return Ok(members.ToMemberResponseDTOs());
        }

        // ==========================================
        // SOI-MÊME OU ADMIN : Voir un profil
        // ==========================================
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<MemberResponseDTO>> GetMember([FromRoute] Guid id)
        {
            if (!IsOwnerOrAdmin(id)) return Forbid();

            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound();

            return Ok(member.ToMemberResponseDTO());
        }

        // ==========================================
        // SOI-MÊME OU ADMIN : Modifier son profil
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember([FromRoute] Guid id, [FromBody] UpdateMemberRequestDTO request)
        {
            // 1. Sécurité
            if (!IsOwnerOrAdmin(id)) return Forbid();

            // 2. Validation automatique (souvent gérée par [ApiController], mais tu peux la laisser)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                // 3. TU PASSES DIRECTEMENT LE DTO (request) AU SERVICE
                // Le service s'occupera de vérifier si le membre existe, de valider l'EmailAddress, etc.
                await _memberService.UpdateAsync(id, request);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Member not found" });
            }
            catch (ArgumentException ex)
            {
                // Pour capturer les erreurs de format (EmailAddress, Password, etc.)
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ==========================================
        // SEUL L'ADMIN : Supprimer un membre
        // ==========================================
        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteMember([FromRoute] Guid id)
        {
            try
            {
                await _memberService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // ==========================================
        // MÉTHODE PRIVÉE : Vérification de l'identité
        // ==========================================
        private bool IsOwnerOrAdmin(Guid targetId)
        {
            // 1. L'admin a tous les droits
            if (User.IsInRole("Admin")) return true;

            // 2. On récupère l'ID stocké dans le Token (Claim NameIdentifier)
            var currentUserIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(currentUserIdClaim, out Guid currentUserId))
            {
                return currentUserId == targetId;
            }

            return false;
        }
    }
}