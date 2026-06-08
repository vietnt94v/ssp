using Ssp.Cmms.Domain.Common;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Entities;

public class User : AuditableEntity
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public Role Role { get; set; }
    public bool IsActive { get; set; } = true;

    public Technician? Technician { get; set; }
}
