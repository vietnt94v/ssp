using Ssp.Cmms.Domain.Common;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Entities;

public class MaintenanceSchedule : AuditableEntity
{
    public Guid EquipmentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public ScheduleFrequency Frequency { get; set; }
    public int IntervalValue { get; set; } = 1;
    public decimal? MeterThreshold { get; set; }
    public DateOnly NextDueDate { get; set; }
    public bool IsActive { get; set; } = true;

    public Equipment? Equipment { get; set; }
    public ICollection<WorkOrder> GeneratedWorkOrders { get; set; } =
        new List<WorkOrder>();
}
