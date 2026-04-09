using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventAssos.Domain.ValueObjects;
using EventAssos.Application.Mappers;

namespace EventAssos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Il faut être connecté pour accéder à ces routes
    public class MembersController(IMemberService _memberService) : ControllerBase
    {
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
            if (!IsOwnerOrAdmin(id)) return Forbid();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var member = await _memberService.GetByIdAsync(id);
            if (member == null) return NotFound(new { Message = "Member not found" });

            member.EmailAddress = EmailAddress.Create(request.EmailAddress);
            member.Pseudo = request.Pseudo;

            await _memberService.UpdateAsync(id, member);
            return NoContent();
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