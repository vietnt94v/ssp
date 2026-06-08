using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;
using Ssp.Cmms.Domain.Services;

namespace Ssp.Cmms.Application.Features.WorkOrders.Commands;

public record ChangeWorkOrderStatusCommand(Guid Id, WorkOrderStatus Status)
    : IRequest<WorkOrderDto>;

public class ChangeWorkOrderStatusCommandValidator
    : AbstractValidator<ChangeWorkOrderStatusCommand>
{
    public ChangeWorkOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class ChangeWorkOrderStatusCommandHandler
    : IRequestHandler<ChangeWorkOrderStatusCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _db;
    private readonly IRealtimeNotifier _notifier;

    public ChangeWorkOrderStatusCommandHandler(
        IApplicationDbContext db,
        IRealtimeNotifier notifier)
    {
        _db = db;
        _notifier = notifier;
    }

    public async Task<WorkOrderDto> Handle(
        ChangeWorkOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var wo = await _db.WorkOrders
            .Include(w => w.Equipment)
            .Include(w => w.AssignedTechnician)
            .Include(w => w.Checklist)
            .Include(w => w.Parts).ThenInclude(p => p.SparePart)
            .Include(w => w.Costs)
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (wo is null)
        {
            throw new NotFoundException(nameof(WorkOrder), request.Id);
        }

        if (wo.Status != request.Status &&
            !WorkOrderStatusMachine.CanTransition(wo.Status, request.Status))
        {
            throw new BusinessRuleException(
                $"Cannot transition work order from {wo.Status} to {request.Status}.");
        }

        var previous = wo.Status;
        wo.Status = request.Status;

        if (request.Status == WorkOrderStatus.InProgress && wo.StartedAt is null)
        {
            wo.StartedAt = DateTimeOffset.UtcNow;
        }

        if (request.Status == WorkOrderStatus.Completed)
        {
            wo.CompletedAt = DateTimeOffset.UtcNow;

            // Record a maintenance log entry on completion.
            var downtime = wo.StartedAt is not null
                ? (int)(wo.CompletedAt.Value - wo.StartedAt.Value).TotalMinutes
                : 0;
            _db.MaintenanceLogs.Add(new MaintenanceLog
            {
                EquipmentId = wo.EquipmentId,
                WorkOrderId = wo.Id,
                CompletedAt = wo.CompletedAt.Value,
                Summary = wo.Description,
                DowntimeMinutes = downtime
            });

            var equipment = await _db.Equipment
                .FirstOrDefaultAsync(e => e.Id == wo.EquipmentId, cancellationToken);
            if (equipment is not null)
            {
                equipment.LastMaintenanceAt = wo.CompletedAt;
            }
        }

        await _db.SaveChangesAsync(cancellationToken);

        var dto = WorkOrderMapping.ToDto(wo);

        if (previous != wo.Status)
        {
            await _notifier.WorkOrderStatusChangedAsync(dto, cancellationToken);
            await _notifier.DashboardKpiUpdatedAsync(cancellationToken);
        }

        return dto;
    }
}
