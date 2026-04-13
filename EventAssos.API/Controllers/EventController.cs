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
    [Authorize]
    public class EventsController(IEventService _eventService) : ControllerBase
    {
        // ===============================
        // GET: api/Events
        // ===============================
        [HttpGet]
        [AllowAnonymous]
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
        [AllowAnonymous]
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
        // POST: api/Events
        // ===============================
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<EventResponseDTO>> PostEvent([FromBody] AddEventRequestDTO request) // Vérifie le type ici
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // On appelle la méthode ToEvent() définie dans ton mapper
            var eventEntity = request.ToEvent();

            // On passe l'entité au service
            var result = await _eventService.CreateAsync(eventEntity);

            // On utilise ton autre extension pour la réponse
            return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result.ToEventResponseDTO());
        }

        // ===============================
        // PUT: api/Events/{id}
        // ===============================
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutEvent([FromRoute] Guid id, [FromBody] UpdateEventRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // On envoie directement le DTO (request) au service
                await _eventService.UpdateAsync(id, request);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Event not found" });
            }
            catch (ArgumentException ex)
            {
                // Au cas où tes Value Objects (ex: EventDate) rejettent la donnée
                return BadRequest(new { Message = ex.Message });
            }
        }

        // ===============================
        // DELETE: api/Events/{id}
        // ===============================
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
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