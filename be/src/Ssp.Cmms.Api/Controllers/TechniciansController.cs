using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/technicians")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class TechniciansController : ControllerBase
{
    private const int WorkloadCapacity = 5;

    private static readonly WorkOrderStatus[] OpenStatuses =
    {
        WorkOrderStatus.Draft,
        WorkOrderStatus.Assigned,
        WorkOrderStatus.InProgress,
        WorkOrderStatus.OnHold
    };

    private readonly IApplicationDbContext _db;

    public TechniciansController(IApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var query = _db.Technicians
            .AsNoTracking()
            .Include(t => t.TechnicianSkills)
                .ThenInclude(ts => ts.Skill)
            .AsQueryable();

        var total = await query.CountAsync(ct);
        var technicians = await query
            .OrderBy(t => t.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var ids = technicians.Select(t => t.Id).ToList();
        var openCounts = await _db.WorkOrders
            .Where(w => w.AssignedTechnicianId != null
                && ids.Contains(w.AssignedTechnicianId.Value)
                && OpenStatuses.Contains(w.Status))
            .GroupBy(w => w.AssignedTechnicianId!.Value)
            .Select(g => new { TechnicianId = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var items = technicians.Select(t =>
        {
            var open = openCounts.FirstOrDefault(c => c.TechnicianId == t.Id)?.Count ?? 0;
            return Project(t, open);
        });

        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var technician = await _db.Technicians
            .AsNoTracking()
            .Include(t => t.TechnicianSkills)
                .ThenInclude(ts => ts.Skill)
            .FirstOrDefaultAsync(t => t.Id == id, ct)
            ?? throw new NotFoundException(nameof(Technician), id);

        var open = await _db.WorkOrders
            .CountAsync(w => w.AssignedTechnicianId == id
                && OpenStatuses.Contains(w.Status), ct);

        return Ok(Project(technician, open));
    }

    [HttpGet("{id:guid}/workload")]
    public async Task<IActionResult> Workload(Guid id, CancellationToken ct)
    {
        var byStatus = await _db.WorkOrders
            .Where(w => w.AssignedTechnicianId == id
                && OpenStatuses.Contains(w.Status))
            .GroupBy(w => w.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var open = byStatus.Sum(s => s.Count);

        return Ok(new
        {
            technicianId = id,
            openWorkOrderCount = open,
            workloadPercent = Math.Min(100, open * 100 / WorkloadCapacity),
            byStatus = byStatus.ToDictionary(s => s.Status.ToString(), s => s.Count)
        });
    }

    private static object Project(Technician t, int openCount) => new
    {
        t.Id,
        t.UserId,
        t.Name,
        department = t.Department,
        skills = t.TechnicianSkills
            .Where(ts => ts.Skill != null)
            .Select(ts => ts.Skill!.Name)
            .ToList(),
        openWorkOrderCount = openCount,
        workloadPercent = Math.Min(100, openCount * 100 / WorkloadCapacity),
        t.Rating,
        t.CreatedAt,
        t.CreatedBy,
        t.UpdatedAt,
        t.UpdatedBy
    };
}
