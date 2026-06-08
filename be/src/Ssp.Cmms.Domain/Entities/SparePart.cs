using Ssp.Cmms.Domain.Common;

namespace Ssp.Cmms.Domain.Entities;

public class SparePart : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal UnitCost { get; set; }
    public int StockQuantity { get; set; }
    public int ReorderLevel { get; set; }

    public ICollection<WorkOrderPart> WorkOrderParts { get; set; } =
        new List<WorkOrderPart>();
}
