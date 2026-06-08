namespace Ssp.Cmms.Infrastructure.Auth;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "Ssp.Cmms";
    public string Audience { get; set; } = "Ssp.Cmms.Client";
    public string SigningKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 60;
}
