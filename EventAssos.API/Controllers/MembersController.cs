using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.Mappers;
using EventAssos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventAssos.Domain.ValueObjects;

namespace EventAssos.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MembersController(IMemberService _memberService) : ControllerBase
    {
        // GET: api/Members
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MemberResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<MemberResponseDTO>>> GetMembers()
        {
            var member = await _memberService.GetAllAsync();
            return Ok(member.ToMemberResponseDTOs());
        }

        // GET:
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(MemberResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<MemberResponseDTO>> GetMember([FromRoute] Guid id)
        {
            var existingMember = await _memberService.GetByIdAsync(id);
            if (existingMember == null)
                return NotFound();

            //return Ok(UserMapperExtensions.ToUserResponseDto(existingUser));
            return Ok(existingMember.ToMemberResponseDTO());
        }

        // PUT:
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

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

        // DELETE:
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMember([FromRoute] Guid id)
        {
            try
            {
                await _memberService.DeleteAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}

