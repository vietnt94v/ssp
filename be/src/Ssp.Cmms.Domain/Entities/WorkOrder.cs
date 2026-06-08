using Ssp.Cmms.Domain.Common;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Entities;

public class WorkOrder : SoftDeletableEntity
{
    public string Number { get; set; } = string.Empty;
    public WorkOrderType Type { get; set; }
    public WorkOrderPriority Priority { get; set; } = WorkOrderPriority.Medium;
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Draft;
    public Guid EquipmentId { get; set; }
    public Guid? AssignedTechnicianId { get; set; }
    public Guid? ScheduleId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset? Deadline { get; set; }
    public DateTimeOffset? StartedAt { get; set; }
    public DateTimeOffset? CompletedAt { get; set; }

    public Equipment? Equipment { get; set; }
    public Technician? AssignedTechnician { get; set; }
    public MaintenanceSchedule? Schedule { get; set; }
    public ICollection<WorkOrderChecklistItem> Checklist { get; set; } =
        new List<WorkOrderChecklistItem>();
    public ICollection<WorkOrderPart> Parts { get; set; } =
        new List<WorkOrderPart>();
    public ICollection<CostEntry> Costs { get; set; } = new List<CostEntry>();
}

public class WorkOrderChecklistItem : BaseEntity
{
    public Guid WorkOrderId { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public int SortOrder { get; set; }

    public WorkOrder? WorkOrder { get; set; }
}

public class WorkOrderPart : BaseEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid SparePartId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitCost { get; set; }

    public WorkOrder? WorkOrder { get; set; }
    public SparePart? SparePart { get; set; }
}
