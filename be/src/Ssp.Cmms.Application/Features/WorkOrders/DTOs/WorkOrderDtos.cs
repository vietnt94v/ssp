using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Application.Features.WorkOrders.DTOs;

public record WorkOrderChecklistItemDto(Guid Id, string Description, bool IsDone);

public record WorkOrderPartDto(
    Guid Id,
    Guid SparePartId,
    string? PartName,
    int Quantity,
    decimal UnitCost);

public record CostEntryDto(
    Guid Id,
    CostEntryType Type,
    decimal Amount,
    string? Description);

public record WorkOrderDto(
    Guid Id,
    string Number,
    WorkOrderType Type,
    WorkOrderPriority Priority,
    WorkOrderStatus Status,
    Guid EquipmentId,
    string? EquipmentName,
    Guid? AssignedTechnicianId,
    string? AssignedTechnicianName,
    string Description,
    DateTimeOffset? Deadline,
    IReadOnlyList<WorkOrderChecklistItemDto> Checklist,
    IReadOnlyList<WorkOrderPartDto> Parts,
    IReadOnlyList<CostEntryDto> Costs,
    decimal TotalCost,
    DateTimeOffset CreatedAt,
    string? CreatedBy,
    DateTimeOffset? UpdatedAt,
    string? UpdatedBy);

public record WorkOrderListItemDto(
    Guid Id,
    string Number,
    WorkOrderType Type,
    WorkOrderPriority Priority,
    WorkOrderStatus Status,
    string? EquipmentName,
    string? AssignedTechnicianName,
    DateTimeOffset? Deadline);

public record ChecklistItemInput(string Description, bool IsDone);

public record CreateWorkOrderDto(
    WorkOrderType Type,
    WorkOrderPriority Priority,
    Guid EquipmentId,
    Guid? AssignedTechnicianId,
    string Description,
    DateTimeOffset? Deadline,
    IReadOnlyList<ChecklistItemInput>? Checklist);
