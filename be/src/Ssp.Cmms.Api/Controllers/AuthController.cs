using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ssp.Cmms.Infrastructure.Auth;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthUserDto>> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        var user = await _auth.LoginAsync(
            request.Email,
            request.Password,
            HttpContext,
            ct);

        return Ok(user);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(CancellationToken ct)
    {
        await _auth.RefreshAsync(HttpContext, ct);
        return NoContent();
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        await _auth.LogoutAsync(HttpContext, ct);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<AuthUserDto>> Me(CancellationToken ct)
    {
        var user = await _auth.GetCurrentUserAsync(HttpContext, ct);
        return Ok(user);
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public IActionResult ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        // Password reset email dispatch is out of scope for the scaffold;
        // always return 204 to avoid leaking which emails exist.
        _ = request;
        return NoContent();
    }
}

public record LoginRequest(string Email, string Password);

public record ForgotPasswordRequest(string Email);
