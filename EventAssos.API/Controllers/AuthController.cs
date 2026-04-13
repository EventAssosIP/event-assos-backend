using EventAssos.Application.DTOs.Requests;
using EventAssos.Application.DTOs.Responses;
using EventAssos.Application.Interfaces.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EventAssos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterMemberRequestDTO request)
        {
            try
            {
                var createdMember = await _authService.Register(request);

                return CreatedAtAction(
                    actionName: "GetMember",
                    controllerName: "Members",
                    routeValues: new { id = createdMember.Id },
                    value: createdMember
                    );
            }
            catch (Exception ex)
            {
                return Conflict(new { ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginMemberResponseDTO>> Login([FromBody] LoginMemberRequestDTO request)
        {
            try
            {
                var loginResponse = await _authService.Login(request);
                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

    }
}
