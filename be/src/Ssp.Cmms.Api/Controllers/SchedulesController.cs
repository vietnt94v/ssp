using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/schedules")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class SchedulesController : ControllerBase
{
    private readonly IApplicationDbContext _db;

    public SchedulesController(IApplicationDbContext db)
    {
        _db = db;
    }

    public record ScheduleInput(
        Guid EquipmentId,
        string Title,
        ScheduleFrequency Frequency,
        int IntervalValue,
        decimal? MeterThreshold,
        DateOnly NextDueDate,
        bool IsActive);

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var query = _db.MaintenanceSchedules
            .AsNoTracking()
            .Include(s => s.Equipment)
            .AsQueryable();

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(s => s.NextDueDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => Project(s))
            .ToListAsync(ct);

        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpGet("calendar")]
    public async Task<IActionResult> Calendar(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to,
        CancellationToken ct)
    {
        var events = await _db.MaintenanceSchedules
            .AsNoTracking()
            .Include(s => s.Equipment)
            .Where(s => s.IsActive
                && s.NextDueDate >= from && s.NextDueDate <= to)
            .Select(s => new
            {
                s.Id,
                s.Title,
                s.EquipmentId,
                equipmentName = s.Equipment != null ? s.Equipment.Name : null,
                s.Frequency,
                start = s.NextDueDate.ToString("yyyy-MM-dd"),
                allDay = true
            })
            .ToListAsync(ct);

        return Ok(events);
    }

    [HttpPost]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Create(
        [FromBody] ScheduleInput input,
        CancellationToken ct)
    {
        var entity = new MaintenanceSchedule
        {
            EquipmentId = input.EquipmentId,
            Title = input.Title,
            Frequency = input.Frequency,
            IntervalValue = input.IntervalValue,
            MeterThreshold = input.MeterThreshold,
            NextDueDate = input.NextDueDate,
            IsActive = input.IsActive
        };
        _db.MaintenanceSchedules.Add(entity);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(List), new { id = entity.Id },
            new { id = entity.Id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] ScheduleInput input,
        CancellationToken ct)
    {
        var entity = await _db.MaintenanceSchedules
            .FirstOrDefaultAsync(s => s.Id == id, ct)
            ?? throw new NotFoundException(nameof(MaintenanceSchedule), id);

        entity.EquipmentId = input.EquipmentId;
        entity.Title = input.Title;
        entity.Frequency = input.Frequency;
        entity.IntervalValue = input.IntervalValue;
        entity.MeterThreshold = input.MeterThreshold;
        entity.NextDueDate = input.NextDueDate;
        entity.IsActive = input.IsActive;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    private static object Project(MaintenanceSchedule s) => new
    {
        s.Id,
        s.EquipmentId,
        equipmentName = s.Equipment != null ? s.Equipment.Name : null,
        s.Title,
        s.Frequency,
        s.IntervalValue,
        s.MeterThreshold,
        nextDueDate = s.NextDueDate.ToString("yyyy-MM-dd"),
        s.IsActive,
        s.CreatedAt,
        s.CreatedBy,
        s.UpdatedAt,
        s.UpdatedBy
    };
}
