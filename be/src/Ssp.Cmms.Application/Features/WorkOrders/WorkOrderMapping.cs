using Ssp.Cmms.Application.Features.WorkOrders.DTOs;
using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Application.Features.WorkOrders;

public static class WorkOrderMapping
{
    public static WorkOrderDto ToDto(WorkOrder w)
    {
        var partsCost = w.Parts.Sum(p => p.Quantity * p.UnitCost);
        var costsTotal = w.Costs.Sum(c => c.Amount);

        return new WorkOrderDto(
            w.Id,
            w.Number,
            w.Type,
            w.Priority,
            w.Status,
            w.EquipmentId,
            w.Equipment?.Name,
            w.AssignedTechnicianId,
            w.AssignedTechnician?.Name,
            w.Description,
            w.Deadline,
            w.Checklist
                .OrderBy(c => c.SortOrder)
                .Select(c => new WorkOrderChecklistItemDto(
                    c.Id, c.Description, c.IsDone))
                .ToList(),
            w.Parts
                .Select(p => new WorkOrderPartDto(
                    p.Id, p.SparePartId, p.SparePart?.Name, p.Quantity, p.UnitCost))
                .ToList(),
            w.Costs
                .Select(c => new CostEntryDto(
                    c.Id, c.Type, c.Amount, c.Description))
                .ToList(),
            partsCost + costsTotal,
            w.CreatedAt,
            w.CreatedBy,
            w.UpdatedAt,
            w.UpdatedBy);
    }
}
