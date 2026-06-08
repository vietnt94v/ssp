using Ssp.Cmms.Domain.Common;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Entities;

public class CostEntry : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public CostEntryType Type { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }

    public WorkOrder? WorkOrder { get; set; }
}
