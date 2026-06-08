using Ssp.Cmms.Domain.Common;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Entities;

public class Equipment : SoftDeletableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public Guid LocationId { get; set; }
    public string? Manufacturer { get; set; }
    public DateOnly? InstallDate { get; set; }
    public EquipmentStatus Status { get; set; } = EquipmentStatus.Active;
    public DateTimeOffset? LastMaintenanceAt { get; set; }
    public string? ImageUrl { get; set; }

    public Category? Category { get; set; }
    public Location? Location { get; set; }
    public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
    public ICollection<MaintenanceSchedule> Schedules { get; set; } =
        new List<MaintenanceSchedule>();
    public ICollection<MaintenanceLog> Logs { get; set; } =
        new List<MaintenanceLog>();
}
