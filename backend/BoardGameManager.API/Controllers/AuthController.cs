using BoardGameManager.Application.DTOs.Auth;
using BoardGameManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BoardGameManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;
  private readonly ILogger<AuthController> _logger;

  public AuthController(IAuthService authService, ILogger<AuthController> logger)
  {
    _authService = authService;
    _logger = logger;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest request)
  {
    try
    {
      var response = await _authService.RegisterAsync(request);
      return Ok(response);
    }
    catch (InvalidOperationException ex)
    {
      return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error during registration");
      return StatusCode(500, new { message = "An error occurred during registration" });
    }
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request)
  {
    try
    {
      var response = await _authService.LoginAsync(request);
      return Ok(response);
    }
    catch (UnauthorizedAccessException ex)
    {
      return Unauthorized(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error during login");
      return StatusCode(500, new { message = "An error occurred during login" });
    }
  }

  [Authorize]
  [HttpGet("me")]
  public async Task<IActionResult> GetCurrentUser()
  {
    try
    {
      var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
      if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
      {
        return Unauthorized();
      }

      var user = await _authService.GetUserByIdAsync(userId);
      if (user == null)
      {
        return NotFound();
      }

      return Ok(user);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting current user");
      return StatusCode(500, new { message = "An error occurred" });
    }
  }
}