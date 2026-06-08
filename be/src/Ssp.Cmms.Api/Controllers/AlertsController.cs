using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/alerts")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class AlertsController : ControllerBase
{
    private readonly IApplicationDbContext _db;
    private readonly ICurrentUser _currentUser;

    public AlertsController(IApplicationDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] bool? acknowledged,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var query = _db.Alerts.AsNoTracking().AsQueryable();
        if (acknowledged is not null)
        {
            query = query.Where(a => a.IsAcknowledged == acknowledged);
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(a => a.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new
            {
                a.Id,
                a.Type,
                a.Message,
                a.EntityId,
                a.EntityType,
                a.IsAcknowledged,
                a.CreatedAt
            })
            .ToListAsync(ct);

        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpPatch("{id:guid}/acknowledge")]
    public async Task<IActionResult> Acknowledge(Guid id, CancellationToken ct)
    {
        var alert = await _db.Alerts.FirstOrDefaultAsync(a => a.Id == id, ct)
            ?? throw new NotFoundException(nameof(Alert), id);

        alert.IsAcknowledged = true;
        alert.AcknowledgedAt = DateTimeOffset.UtcNow;
        alert.AcknowledgedBy = _currentUser.Email;
        await _db.SaveChangesAsync(ct);

        return Ok(new
        {
            alert.Id,
            alert.Type,
            alert.Message,
            alert.EntityId,
            alert.EntityType,
            alert.IsAcknowledged,
            alert.CreatedAt
        });
    }

    [HttpPost("{id:guid}/create-wo")]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> CreateWorkOrder(Guid id, CancellationToken ct)
    {
        var alert = await _db.Alerts.FirstOrDefaultAsync(a => a.Id == id, ct)
            ?? throw new NotFoundException(nameof(Alert), id);

        if (alert.EntityType != nameof(Equipment) || alert.EntityId is null)
        {
            throw new BusinessRuleException(
                "This alert is not linked to a piece of equipment.");
        }

        var workOrder = new WorkOrder
        {
            Number = await GenerateNumberAsync(ct),
            Type = WorkOrderType.Corrective,
            Priority = WorkOrderPriority.High,
            Status = WorkOrderStatus.Draft,
            EquipmentId = alert.EntityId.Value,
            Description = alert.Message
        };
        _db.WorkOrders.Add(workOrder);

        alert.IsAcknowledged = true;
        alert.AcknowledgedAt = DateTimeOffset.UtcNow;
        alert.AcknowledgedBy = _currentUser.Email;

        await _db.SaveChangesAsync(ct);

        return Ok(new { workOrderId = workOrder.Id });
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
