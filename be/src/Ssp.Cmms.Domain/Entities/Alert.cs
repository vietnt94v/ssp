using Ssp.Cmms.Domain.Common;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Entities;

public class Alert : BaseEntity
{
    public AlertType Type { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? EntityId { get; set; }
    public string? EntityType { get; set; }
    public bool IsAcknowledged { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? AcknowledgedAt { get; set; }
    public string? AcknowledgedBy { get; set; }
}
