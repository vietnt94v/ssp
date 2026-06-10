using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ssp.Cmms.Infrastructure.Auth;

public class AuthCookieFactory
{
    private readonly AuthCookieSettings _settings;
    private readonly JwtSettings _jwt;

    public AuthCookieFactory(
        IOptions<AuthCookieSettings> settings,
        IOptions<JwtSettings> jwt)
    {
        _settings = settings.Value;
        _jwt = jwt.Value;
    }

    private SameSiteMode SameSite =>
        _settings.CrossSite ? SameSiteMode.None : SameSiteMode.Lax;

    private bool Secure => _settings.CrossSite;

    public CookieOptions AccessTokenOptions() => new()
    {
        HttpOnly = true,
        Secure = Secure,
        SameSite = SameSite,
        Domain = _settings.Domain,
        Path = "/",
        MaxAge = TimeSpan.FromMinutes(_jwt.AccessTokenMinutes),
    };

    public CookieOptions RefreshTokenOptions() => new()
    {
        HttpOnly = true,
        Secure = Secure,
        SameSite = SameSite,
        Domain = _settings.Domain,
        Path = AuthCookieSettings.RefreshTokenPath,
        MaxAge = TimeSpan.FromDays(_jwt.RefreshTokenDays),
    };

    public CookieOptions ExpiredAccessTokenOptions()
    {
        var options = AccessTokenOptions();
        options.MaxAge = TimeSpan.Zero;
        return options;
    }

    public CookieOptions ExpiredRefreshTokenOptions()
    {
        var options = RefreshTokenOptions();
        options.MaxAge = TimeSpan.Zero;
        return options;
    }
}
