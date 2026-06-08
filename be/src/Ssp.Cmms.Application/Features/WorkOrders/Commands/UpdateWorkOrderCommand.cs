using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;

namespace Ssp.Cmms.Application.Features.WorkOrders.Commands;

public record UpdateWorkOrderCommand(Guid Id, CreateWorkOrderDto Data)
    : IRequest<WorkOrderDto>;

public class UpdateWorkOrderCommandValidator
    : AbstractValidator<UpdateWorkOrderCommand>
{
    public UpdateWorkOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Data.Description).NotEmpty().MaximumLength(2000);
    }
}

public class UpdateWorkOrderCommandHandler
    : IRequestHandler<UpdateWorkOrderCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _db;

    public UpdateWorkOrderCommandHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<WorkOrderDto> Handle(
        UpdateWorkOrderCommand request,
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
            throw new NotFoundException(nameof(Domain.Entities.WorkOrder), request.Id);
        }

        var data = request.Data;
        wo.Type = data.Type;
        wo.Priority = data.Priority;
        wo.EquipmentId = data.EquipmentId;
        wo.AssignedTechnicianId = data.AssignedTechnicianId;
        wo.Description = data.Description;
        wo.Deadline = data.Deadline;

        await _db.SaveChangesAsync(cancellationToken);

        return WorkOrderMapping.ToDto(wo);
    }
}
