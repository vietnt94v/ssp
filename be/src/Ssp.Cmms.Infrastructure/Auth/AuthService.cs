using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _tokens;
    private readonly AuthCookieFactory _cookies;
    private readonly JwtSettings _jwt;

    public AuthService(
        IApplicationDbContext db,
        IPasswordHasher hasher,
        IJwtTokenService tokens,
        AuthCookieFactory cookies,
        IOptions<JwtSettings> jwt)
    {
        _db = db;
        _hasher = hasher;
        _tokens = tokens;
        _cookies = cookies;
        _jwt = jwt.Value;
    }

    public async Task<AuthUserDto> LoginAsync(
        string email,
        string password,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive, ct);

        if (user is null || !_hasher.Verify(password, user.PasswordHash))
        {
            throw new UnauthorizedException("Invalid email or password.");
        }

        await IssueTokensAsync(user, Guid.NewGuid(), httpContext, ct);

        return ToDto(user);
    }

    public async Task RefreshAsync(HttpContext httpContext, CancellationToken ct)
    {
        var presented =
            httpContext.Request.Cookies[AuthCookieSettings.RefreshTokenName];

        if (string.IsNullOrEmpty(presented))
        {
            ClearCookies(httpContext);
            throw new UnauthorizedException("Missing refresh token.");
        }

        var hash = _tokens.HashRefreshToken(presented);
        var stored = await _db.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hash, ct);

        if (stored is null)
        {
            ClearCookies(httpContext);
            throw new UnauthorizedException("Invalid refresh token.");
        }

        // Reuse detection: a token that was already rotated/revoked is being
        // presented again. Treat as compromise and revoke the entire family.
        if (stored.IsRevoked)
        {
            await RevokeFamilyAsync(stored.FamilyId, ct);
            ClearCookies(httpContext);
            throw new UnauthorizedException(
                "Refresh token reuse detected. Session revoked.");
        }

        if (stored.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            stored.IsRevoked = true;
            await _db.SaveChangesAsync(ct);
            ClearCookies(httpContext);
            throw new UnauthorizedException("Refresh token expired.");
        }

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == stored.UserId && u.IsActive, ct);

        if (user is null)
        {
            await RevokeFamilyAsync(stored.FamilyId, ct);
            ClearCookies(httpContext);
            throw new UnauthorizedException("User no longer active.");
        }

        // Rotation: invalidate the presented token, issue a fresh one in the
        // same family.
        stored.IsRevoked = true;
        await IssueTokensAsync(user, stored.FamilyId, httpContext, ct);
    }

    public async Task LogoutAsync(HttpContext httpContext, CancellationToken ct)
    {
        var presented =
            httpContext.Request.Cookies[AuthCookieSettings.RefreshTokenName];

        if (!string.IsNullOrEmpty(presented))
        {
            var hash = _tokens.HashRefreshToken(presented);
            var stored = await _db.RefreshTokens
                .FirstOrDefaultAsync(t => t.TokenHash == hash, ct);

            if (stored is not null)
            {
                await RevokeFamilyAsync(stored.FamilyId, ct);
            }
        }

        ClearCookies(httpContext);
    }

    public async Task<AuthUserDto> GetCurrentUserAsync(
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userIdValue =
            httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            throw new UnauthorizedException("Not authenticated.");
        }

        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsActive, ct);

        if (user is null)
        {
            throw new UnauthorizedException("User no longer active.");
        }

        return ToDto(user);
    }

    private async Task IssueTokensAsync(
        User user,
        Guid familyId,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var accessToken = _tokens.CreateAccessToken(user);
        var refreshToken = _tokens.CreateRefreshToken();

        var entity = new RefreshToken
        {
            TokenHash = _tokens.HashRefreshToken(refreshToken),
            UserId = user.Id,
            FamilyId = familyId,
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(_jwt.RefreshTokenDays),
            IsRevoked = false,
            CreatedAt = DateTimeOffset.UtcNow,
            DeviceInfo = httpContext.Request.Headers.UserAgent.ToString(),
        };

        _db.RefreshTokens.Add(entity);
        await _db.SaveChangesAsync(ct);

        httpContext.Response.Cookies.Append(
            AuthCookieSettings.AccessTokenName,
            accessToken,
            _cookies.AccessTokenOptions());

        httpContext.Response.Cookies.Append(
            AuthCookieSettings.RefreshTokenName,
            refreshToken,
            _cookies.RefreshTokenOptions());
    }

    private async Task RevokeFamilyAsync(Guid familyId, CancellationToken ct)
    {
        var family = await _db.RefreshTokens
            .Where(t => t.FamilyId == familyId && !t.IsRevoked)
            .ToListAsync(ct);

        foreach (var token in family)
        {
            token.IsRevoked = true;
        }

        if (family.Count > 0)
        {
            await _db.SaveChangesAsync(ct);
        }
    }

    private void ClearCookies(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Append(
            AuthCookieSettings.AccessTokenName,
            string.Empty,
            _cookies.ExpiredAccessTokenOptions());

        httpContext.Response.Cookies.Append(
            AuthCookieSettings.RefreshTokenName,
            string.Empty,
            _cookies.ExpiredRefreshTokenOptions());
    }

    private static AuthUserDto ToDto(User user) =>
        new(user.Id, user.Email, user.FullName, user.Role);
}
