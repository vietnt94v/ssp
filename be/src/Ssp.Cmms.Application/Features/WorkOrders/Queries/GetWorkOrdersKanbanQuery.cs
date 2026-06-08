using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Application.Features.WorkOrders.Queries;

public record GetWorkOrdersKanbanQuery(Guid? TechnicianId)
    : IRequest<Dictionary<string, List<WorkOrderListItemDto>>>;

public class GetWorkOrdersKanbanQueryHandler
    : IRequestHandler<GetWorkOrdersKanbanQuery,
        Dictionary<string, List<WorkOrderListItemDto>>>
{
    private readonly IApplicationDbContext _db;

    public GetWorkOrdersKanbanQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Dictionary<string, List<WorkOrderListItemDto>>> Handle(
        GetWorkOrdersKanbanQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.WorkOrders.AsNoTracking();

        if (request.TechnicianId is not null)
            query = query.Where(w => w.AssignedTechnicianId == request.TechnicianId);

        var items = await query
            .Select(w => new WorkOrderListItemDto(
                w.Id,
                w.Number,
                w.Type,
                w.Priority,
                w.Status,
                w.Equipment!.Name,
                w.AssignedTechnician != null ? w.AssignedTechnician.Name : null,
                w.Deadline))
            .ToListAsync(cancellationToken);

        var board = Enum.GetValues<WorkOrderStatus>()
            .ToDictionary(s => s.ToString(), _ => new List<WorkOrderListItemDto>());

        foreach (var item in items)
        {
            board[item.Status.ToString()].Add(item);
        }

        return board;
    }
}
