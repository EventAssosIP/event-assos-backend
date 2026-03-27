using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventAssos.Domain.ValueObjects;
using EventAssos.Domain.Entities;

namespace EventAssos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MembersController(IMemberService _memberService) : ControllerBase
    {
        // ===============================
        // GET: api/Members
        // ===============================
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MemberResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetMembers()
        {
            var members = await _memberService.GetAllAsync();
            return Ok(ToMemberResponseDTOs(members));
        }

        // ===============================
        // GET: api/Members/{id}
        // ===============================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MemberResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MemberResponseDTO>> GetMember([FromRoute] Guid id)
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
                return NotFound();

            return Ok(ToMemberResponseDTO(member));
        }

        // ===============================
        // PUT: api/Members/{id}
        // ===============================
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMember([FromRoute] Guid id, [FromBody] UpdateMemberRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
                return NotFound(new { Message = "Member not found" });

            member.EmailAddress = EmailAddress.Create(request.EmailAddress);
            member.Pseudo = request.Pseudo;

            await _memberService.UpdateAsync(id, member);

            return NoContent();
        }

        // ===============================
        // DELETE: api/Members/{id}
        // ===============================
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMember([FromRoute] Guid id)
        {
            try
            {
                await _memberService.DeleteAsync(id);
                return NoContent(); // 🔥 correction REST
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        // ===============================
        // MAPPERS PRIVÉS (autonomes)
        // ===============================

        private MemberResponseDTO ToMemberResponseDTO(Member member)
        {
            return new MemberResponseDTO
            {
                Id = member.Id,
                Pseudo = member.Pseudo,
                EmailAddress = member.EmailAddress.ToString(),
                CreatedAt = member.CreatedAt
            };
        }

        private IEnumerable<MemberResponseDTO> ToMemberResponseDTOs(IEnumerable<Member> members)
        {
            return members.Select(m => ToMemberResponseDTO(m));
        }
    }
}