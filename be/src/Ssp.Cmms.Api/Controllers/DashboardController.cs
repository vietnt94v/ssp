using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class DashboardController : ControllerBase
{
    private static readonly WorkOrderStatus[] OpenStatuses =
    {
        WorkOrderStatus.Draft,
        WorkOrderStatus.Assigned,
        WorkOrderStatus.InProgress,
        WorkOrderStatus.OnHold
    };

    private readonly IApplicationDbContext _db;

    public DashboardController(IApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("kpi")]
    public async Task<IActionResult> Kpi(CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var monthStart = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero);

        var openWorkOrders = await _db.WorkOrders
            .CountAsync(w => OpenStatuses.Contains(w.Status), ct);
        var overdue = await _db.WorkOrders
            .CountAsync(w => OpenStatuses.Contains(w.Status)
                && w.Deadline != null && w.Deadline < now, ct);
        var broken = await _db.Equipment
            .CountAsync(e => e.Status == EquipmentStatus.Broken, ct);
        var monthlyCost = await _db.CostEntries
            .Where(c => c.CreatedAt >= monthStart)
            .SumAsync(c => (decimal?)c.Amount, ct) ?? 0m;

        var logs = await _db.MaintenanceLogs
            .Select(l => l.DowntimeMinutes)
            .ToListAsync(ct);
        var mttrHours = logs.Count > 0
            ? Math.Round(logs.Average() / 60.0, 1)
            : 0;

        return Ok(new
        {
            openWorkOrders,
            overdueWorkOrders = overdue,
            brokenEquipment = broken,
            monthlyCost,
            mttrHours,
            mtbfHours = 0.0
        });
    }

    [HttpGet("wo-trend")]
    public async Task<IActionResult> Trend([FromQuery] int days = 30, CancellationToken ct = default)
    {
        var since = new DateTimeOffset(
            DateTime.UtcNow.Date.AddDays(-days + 1), TimeSpan.Zero);

        var createdDates = await _db.WorkOrders
            .Where(w => w.CreatedAt >= since)
            .Select(w => w.CreatedAt)
            .ToListAsync(ct);
        var completedDates = await _db.WorkOrders
            .Where(w => w.CompletedAt != null && w.CompletedAt >= since)
            .Select(w => w.CompletedAt!.Value)
            .ToListAsync(ct);

        var created = createdDates
            .GroupBy(d => d.UtcDateTime.Date)
            .ToDictionary(g => g.Key, g => g.Count());
        var completed = completedDates
            .GroupBy(d => d.UtcDateTime.Date)
            .ToDictionary(g => g.Key, g => g.Count());

        var points = Enumerable.Range(0, days)
            .Select(offset =>
            {
                var date = since.UtcDateTime.Date.AddDays(offset);
                return new
                {
                    date = date.ToString("yyyy-MM-dd"),
                    created = created.GetValueOrDefault(date),
                    completed = completed.GetValueOrDefault(date)
                };
            })
            .ToList();

        return Ok(points);
    }

    [HttpGet("urgent-wo")]
    public async Task<IActionResult> Urgent(CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;
        var items = await _db.WorkOrders
            .AsNoTracking()
            .Include(w => w.Equipment)
            .Include(w => w.AssignedTechnician)
            .Where(w => OpenStatuses.Contains(w.Status)
                && (w.Priority == WorkOrderPriority.High
                    || w.Priority == WorkOrderPriority.Critical
                    || (w.Deadline != null && w.Deadline < now)))
            .OrderByDescending(w => w.Priority)
            .ThenBy(w => w.Deadline)
            .Take(10)
            .Select(w => new
            {
                w.Id,
                w.Number,
                w.Type,
                w.Priority,
                w.Status,
                w.EquipmentId,
                equipmentName = w.Equipment != null ? w.Equipment.Name : null,
                w.AssignedTechnicianId,
                assignedTechnicianName = w.AssignedTechnician != null
                    ? w.AssignedTechnician.Name
                    : null,
                w.Deadline
            })
            .ToListAsync(ct);

        return Ok(items);
    }

    [HttpGet("status-breakdown")]
    public async Task<IActionResult> StatusBreakdown(CancellationToken ct)
    {
        var counts = await _db.WorkOrders
            .GroupBy(w => w.Status)
            .Select(g => new { status = g.Key, count = g.Count() })
            .ToListAsync(ct);
        return Ok(counts);
    }
}
