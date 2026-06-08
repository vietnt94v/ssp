using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ssp.Cmms.Application.Features.Auth;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResult>> Login(
        LoginCommand command,
        CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        return Ok(result);
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

public record ForgotPasswordRequest(string Email);
