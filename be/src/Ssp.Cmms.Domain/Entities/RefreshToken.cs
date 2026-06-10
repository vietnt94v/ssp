using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string TokenHash { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid FamilyId { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string? DeviceInfo { get; set; }

    public User? User { get; set; }

    public bool IsActive => !IsRevoked && ExpiresAt > DateTimeOffset.UtcNow;
}
