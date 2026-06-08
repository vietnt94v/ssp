namespace Ssp.Cmms.Domain.Enums;

public enum Role
{
    Admin,
    Manager,
    Technician
}

public enum EquipmentStatus
{
    Active,
    UnderMaintenance,
    Broken,
    Decommissioned
}

public enum WorkOrderType
{
    Corrective,
    Preventive,
    Inspection
}

public enum WorkOrderStatus
{
    Draft,
    Assigned,
    InProgress,
    OnHold,
    Completed,
    Closed
}

public enum WorkOrderPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum ScheduleFrequency
{
    Daily,
    Weekly,
    Monthly,
    ByMeter
}

public enum AlertType
{
    PmDue,
    EquipmentBreakdown,
    WoOverdue,
    LowStock
}

public enum CostEntryType
{
    Labor,
    Parts
}
