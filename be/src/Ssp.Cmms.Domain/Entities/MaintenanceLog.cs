using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class MaintenanceLog : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public Guid WorkOrderId { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
    public string Summary { get; set; } = string.Empty;
    public int DowntimeMinutes { get; set; }

    public Equipment? Equipment { get; set; }
    public WorkOrder? WorkOrder { get; set; }
}
