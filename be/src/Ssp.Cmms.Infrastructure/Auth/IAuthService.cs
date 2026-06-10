using Microsoft.AspNetCore.Http;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Infrastructure.Auth;

public record AuthUserDto(Guid Id, string Email, string FullName, Role Role);

public interface IAuthService
{
    Task<AuthUserDto> LoginAsync(
        string email,
        string password,
        HttpContext httpContext,
        CancellationToken ct);

    Task RefreshAsync(HttpContext httpContext, CancellationToken ct);

    Task LogoutAsync(HttpContext httpContext, CancellationToken ct);

    Task<AuthUserDto> GetCurrentUserAsync(
        HttpContext httpContext,
        CancellationToken ct);
}
