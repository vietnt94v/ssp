using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Policy = "RequireManagerOrAdmin")]
public class ReportsController : ControllerBase
{
    private readonly IApplicationDbContext _db;

    public ReportsController(IApplicationDbContext db)
    {
        _db = db;
    }

    private static (DateTimeOffset From, DateTimeOffset To) Resolve(
        DateOnly? from, DateOnly? to)
    {
        var toDate = to ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var fromDate = from ?? toDate.AddMonths(-1);
        return (
            new DateTimeOffset(fromDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero),
            new DateTimeOffset(toDate.ToDateTime(TimeOnly.MaxValue), TimeSpan.Zero));
    }

    [HttpGet("kpi")]
    public async Task<IActionResult> Kpi(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
    {
        var (fromTs, toTs) = Resolve(from, to);

        var logs = await _db.MaintenanceLogs
            .Where(l => l.CompletedAt >= fromTs && l.CompletedAt <= toTs)
            .Select(l => l.DowntimeMinutes)
            .ToListAsync(ct);
        var mttrHours = logs.Count > 0 ? Math.Round(logs.Average() / 60.0, 1) : 0;

        var completed = await _db.WorkOrders
            .CountAsync(w => w.CompletedAt >= fromTs && w.CompletedAt <= toTs, ct);
        var overdue = await _db.WorkOrders
            .CountAsync(w => w.Deadline != null && w.CompletedAt != null
                && w.CompletedAt > w.Deadline
                && w.CompletedAt >= fromTs && w.CompletedAt <= toTs, ct);
        var overdueRate = completed > 0
            ? Math.Round(overdue * 100.0 / completed, 1)
            : 0;

        return Ok(new
        {
            from = DateOnly.FromDateTime(fromTs.Date).ToString("yyyy-MM-dd"),
            to = DateOnly.FromDateTime(toTs.Date).ToString("yyyy-MM-dd"),
            mttrHours,
            mtbfHours = 0.0,
            oeeImpactPercent = 0.0,
            overdueRatePercent = overdueRate
        });
    }

    [HttpGet("cost")]
    public async Task<IActionResult> Cost(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
    {
        var (fromTs, toTs) = Resolve(from, to);

        var entries = await _db.CostEntries
            .Where(c => c.CreatedAt >= fromTs && c.CreatedAt <= toTs)
            .Include(c => c.WorkOrder!)
                .ThenInclude(w => w.Equipment)
            .Select(c => new
            {
                c.Type,
                c.Amount,
                EquipmentId = c.WorkOrder!.EquipmentId,
                EquipmentName = c.WorkOrder.Equipment!.Name
            })
            .ToListAsync(ct);

        var byEquipment = entries
            .GroupBy(e => new { e.EquipmentId, e.EquipmentName })
            .Select(g => new
            {
                equipmentId = g.Key.EquipmentId,
                equipmentName = g.Key.EquipmentName,
                laborCost = g.Where(x => x.Type == CostEntryType.Labor).Sum(x => x.Amount),
                partsCost = g.Where(x => x.Type == CostEntryType.Parts).Sum(x => x.Amount),
                totalCost = g.Sum(x => x.Amount)
            })
            .OrderByDescending(x => x.totalCost)
            .ToList();

        return Ok(new
        {
            from = DateOnly.FromDateTime(fromTs.Date).ToString("yyyy-MM-dd"),
            to = DateOnly.FromDateTime(toTs.Date).ToString("yyyy-MM-dd"),
            totalCost = entries.Sum(e => e.Amount),
            laborCost = entries.Where(e => e.Type == CostEntryType.Labor).Sum(e => e.Amount),
            partsCost = entries.Where(e => e.Type == CostEntryType.Parts).Sum(e => e.Amount),
            byEquipment
        });
    }

    [HttpGet("downtime")]
    public async Task<IActionResult> Downtime(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
    {
        var (fromTs, toTs) = Resolve(from, to);

        var byEquipment = await _db.MaintenanceLogs
            .Where(l => l.CompletedAt >= fromTs && l.CompletedAt <= toTs)
            .Include(l => l.Equipment)
            .GroupBy(l => new { l.EquipmentId, EquipmentName = l.Equipment!.Name })
            .Select(g => new
            {
                equipmentId = g.Key.EquipmentId,
                equipmentName = g.Key.EquipmentName,
                downtimeMinutes = g.Sum(x => x.DowntimeMinutes),
                incidents = g.Count()
            })
            .OrderByDescending(x => x.downtimeMinutes)
            .ToListAsync(ct);

        return Ok(new
        {
            from = DateOnly.FromDateTime(fromTs.Date).ToString("yyyy-MM-dd"),
            to = DateOnly.FromDateTime(toTs.Date).ToString("yyyy-MM-dd"),
            byEquipment
        });
    }

    [HttpGet("equipment-history/{equipmentId:guid}")]
    public async Task<IActionResult> EquipmentHistory(
        Guid equipmentId,
        CancellationToken ct)
    {
        var equipment = await _db.Equipment
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == equipmentId, ct)
            ?? throw new NotFoundException(nameof(Equipment), equipmentId);

        var events = await _db.MaintenanceLogs
            .Where(l => l.EquipmentId == equipmentId)
            .Include(l => l.WorkOrder)
            .OrderByDescending(l => l.CompletedAt)
            .Select(l => new
            {
                workOrderNumber = l.WorkOrder != null ? l.WorkOrder.Number : string.Empty,
                type = l.WorkOrder != null ? l.WorkOrder.Type.ToString() : string.Empty,
                completedAt = l.CompletedAt,
                summary = l.Summary,
                downtimeMinutes = l.DowntimeMinutes
            })
            .ToListAsync(ct);

        return Ok(new
        {
            equipmentId,
            equipmentName = equipment.Name,
            events
        });
    }
}
