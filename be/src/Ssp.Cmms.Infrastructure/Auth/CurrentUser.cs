using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _accessor;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

    public string? UserId =>
        Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => Principal?.FindFirstValue(ClaimTypes.Email);

    public Role? Role
    {
        get
        {
            var value = Principal?.FindFirstValue(ClaimTypes.Role);
            return Enum.TryParse<Role>(value, out var role) ? role : null;
        }
    }

    public bool IsAuthenticated =>
        Principal?.Identity?.IsAuthenticated ?? false;
}
