using Fundoo.User.Api.Models;
using Fundoo.User.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fundoo.User.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IUserApplication userApplication) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model, CancellationToken ct)
    {
        try
        {
            // Manually passing fields to application layer as per your instruction
            var result = await userApplication.RegisterAsync(model.FirstName, model.LastName, model.Email, model.Password, ct);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model, CancellationToken ct)
    {
        try
        {
            var result = await userApplication.LoginAsync(model.Email, model.Password, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}