using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.Mappers;
using EventAssos.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventAssos.Domain.Entities;

namespace EventAssos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class EventsController(IEventService _eventService) : ControllerBase
    {
        // ===============================
        // GET: api/Events
        // ===============================
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventResponseDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EventResponseDTO>>> GetEvents()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events.ToEventResponseDTOs());
        }

        // ===============================
        // GET: api/Events/{id}
        // ===============================
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(EventResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EventResponseDTO>> GetEvent([FromRoute] Guid id)
        {
            var Event = await _eventService.GetByIdAsync(id);
            if (Event == null)
                return NotFound();

            return Ok(Event.ToEventResponseDTO());
        }

        // ===============================
        // PUT: api/Events/{id}
        // ===============================
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEvent([FromRoute] Guid id, [FromBody] UpdateEventRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var Event = await _eventService.GetByIdAsync(id);
            if (Event == null)
                return NotFound(new { Message = "Event not found" });

            
            Event.Name = request.Name;

            await _eventService.UpdateAsync(id, Event);

            return NoContent();
        }

        // ===============================
        // DELETE: api/Events/{id}
        // ===============================
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
        {
            try
            {
                await _eventService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }
    }
}