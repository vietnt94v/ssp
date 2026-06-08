using MediatR;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Application.Common.Models;
using Ssp.Cmms.Application.Features.WorkOrders.DTOs;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Application.Features.WorkOrders.Queries;

public class GetWorkOrdersQuery
    : PaginationQuery, IRequest<PaginatedList<WorkOrderListItemDto>>
{
    public WorkOrderType? Type { get; set; }
    public WorkOrderStatus? Status { get; set; }
    public WorkOrderPriority? Priority { get; set; }
    public Guid? TechnicianId { get; set; }
    public Guid? EquipmentId { get; set; }
}

public class GetWorkOrdersQueryHandler
    : IRequestHandler<GetWorkOrdersQuery, PaginatedList<WorkOrderListItemDto>>
{
    private readonly IApplicationDbContext _db;

    public GetWorkOrdersQueryHandler(IApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedList<WorkOrderListItemDto>> Handle(
        GetWorkOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.WorkOrders.AsNoTracking();

        if (request.Type is not null)
            query = query.Where(w => w.Type == request.Type);
        if (request.Status is not null)
            query = query.Where(w => w.Status == request.Status);
        if (request.Priority is not null)
            query = query.Where(w => w.Priority == request.Priority);
        if (request.TechnicianId is not null)
            query = query.Where(w => w.AssignedTechnicianId == request.TechnicianId);
        if (request.EquipmentId is not null)
            query = query.Where(w => w.EquipmentId == request.EquipmentId);

        query = (request.SortBy?.ToLowerInvariant()) switch
        {
            "priority" => request.SortDir == "asc"
                ? query.OrderBy(w => w.Priority)
                : query.OrderByDescending(w => w.Priority),
            "deadline" => request.SortDir == "asc"
                ? query.OrderBy(w => w.Deadline)
                : query.OrderByDescending(w => w.Deadline),
            _ => query.OrderByDescending(w => w.CreatedAt)
        };

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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

        return new PaginatedList<WorkOrderListItemDto>(
            items, total, request.Page, request.PageSize);
    }
}
