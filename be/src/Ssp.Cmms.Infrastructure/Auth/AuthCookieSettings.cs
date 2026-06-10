namespace Ssp.Cmms.Infrastructure.Auth;

public class AuthCookieSettings
{
    public const string SectionName = "AuthCookies";

    public const string AccessTokenName = "access_token";
    public const string RefreshTokenName = "refresh_token";
    public const string RefreshTokenPath = "/api/auth/refresh-token";

    public bool CrossSite { get; set; }

    public string? Domain { get; set; }
}
