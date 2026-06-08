using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;

namespace Ssp.Cmms.Application.Features.WorkOrders.Queries;

public record GetWorkOrderByIdQuery(Guid Id) : IRequest<WorkOrderDto>;

public class GetWorkOrderByIdQueryHandler
    : IRequestHandler<GetWorkOrderByIdQuery, WorkOrderDto>
{
    private readonly IApplicationDbContext _db;

    public GetWorkOrderByIdQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<WorkOrderDto> Handle(
        GetWorkOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var wo = await _db.WorkOrders
            .AsNoTracking()
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

        return WorkOrderMapping.ToDto(wo);
    }
}
