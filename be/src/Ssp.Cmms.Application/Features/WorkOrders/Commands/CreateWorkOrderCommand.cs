using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Application.Features.WorkOrders.Commands;

public record CreateWorkOrderCommand(CreateWorkOrderDto Data)
    : IRequest<WorkOrderDto>;

public class CreateWorkOrderCommandValidator
    : AbstractValidator<CreateWorkOrderCommand>
{
    public CreateWorkOrderCommandValidator()
    {
        RuleFor(x => x.Data.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Data.EquipmentId).NotEmpty();
    }
}

public class CreateWorkOrderCommandHandler
    : IRequestHandler<CreateWorkOrderCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _db;

    public CreateWorkOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<WorkOrderDto> Handle(
        CreateWorkOrderCommand request,
        CancellationToken cancellationToken)
    {
        var data = request.Data;

        var equipmentExists = await _db.Equipment
            .AnyAsync(e => e.Id == data.EquipmentId, cancellationToken);
        if (!equipmentExists)
        {
            throw new NotFoundException(nameof(Equipment), data.EquipmentId);
        }

        var workOrder = new WorkOrder
        {
            Number = await GenerateNumberAsync(cancellationToken),
            Type = data.Type,
            Priority = data.Priority,
            Status = data.AssignedTechnicianId is null
                ? WorkOrderStatus.Draft
                : WorkOrderStatus.Assigned,
            EquipmentId = data.EquipmentId,
            AssignedTechnicianId = data.AssignedTechnicianId,
            Description = data.Description,
            Deadline = data.Deadline
        };

        if (data.Checklist is not null)
        {
            var order = 0;
            foreach (var item in data.Checklist)
            {
                workOrder.Checklist.Add(new WorkOrderChecklistItem
                {
                    Description = item.Description,
                    IsDone = item.IsDone,
                    SortOrder = order++
                });
            }
        }

        _db.WorkOrders.Add(workOrder);
        await _db.SaveChangesAsync(cancellationToken);

        return WorkOrderMapping.ToDto(workOrder);
    }

    private async Task<string> GenerateNumberAsync(CancellationToken ct)
    {
        var year = DateTime.UtcNow.Year;
        var count = await _db.WorkOrders
            .IgnoreQueryFilters()
            .CountAsync(w => w.Number.StartsWith($"WO-{year}-"), ct);
        return $"WO-{year}-{(count + 1):D4}";
    }
}
