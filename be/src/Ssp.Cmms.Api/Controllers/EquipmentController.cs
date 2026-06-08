using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/equipment")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class EquipmentController : ControllerBase
{
    private readonly IApplicationDbContext _db;

    public EquipmentController(IApplicationDbContext db)
    {
        _db = db;
    }

    public record EquipmentInput(
        string Code,
        string Name,
        Guid CategoryId,
        Guid LocationId,
        string? Manufacturer,
        DateOnly? InstallDate,
        EquipmentStatus Status);

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] EquipmentStatus? status,
        [FromQuery] Guid? categoryId,
        [FromQuery] Guid? locationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = _db.Equipment
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Location)
            .AsQueryable();

        if (status is not null) query = query.Where(e => e.Status == status);
        if (categoryId is not null) query = query.Where(e => e.CategoryId == categoryId);
        if (locationId is not null) query = query.Where(e => e.LocationId == locationId);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(e => e.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => Project(e))
            .ToListAsync(ct);

        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var entity = await _db.Equipment
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Location)
            .FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new NotFoundException(nameof(Equipment), id);

        return Ok(Project(entity));
    }

    [HttpPost]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Create(
        [FromBody] EquipmentInput input,
        CancellationToken ct)
    {
        var entity = new Equipment
        {
            Code = input.Code,
            Name = input.Name,
            CategoryId = input.CategoryId,
            LocationId = input.LocationId,
            Manufacturer = input.Manufacturer,
            InstallDate = input.InstallDate,
            Status = input.Status
        };
        _db.Equipment.Add(entity);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id },
            new { id = entity.Id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] EquipmentInput input,
        CancellationToken ct)
    {
        var entity = await _db.Equipment
            .FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new NotFoundException(nameof(Equipment), id);

        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.CategoryId = input.CategoryId;
        entity.LocationId = input.LocationId;
        entity.Manufacturer = input.Manufacturer;
        entity.InstallDate = input.InstallDate;
        entity.Status = input.Status;
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var entity = await _db.Equipment
            .FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new NotFoundException(nameof(Equipment), id);

        _db.Equipment.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return NoContent();
    }

    private static object Project(Equipment e) => new
    {
        e.Id,
        e.Code,
        e.Name,
        e.CategoryId,
        categoryName = e.Category != null ? e.Category.Name : null,
        e.LocationId,
        locationName = e.Location != null ? e.Location.Name : null,
        e.Manufacturer,
        e.InstallDate,
        e.Status,
        e.LastMaintenanceAt,
        e.ImageUrl,
        e.CreatedAt,
        e.CreatedBy,
        e.UpdatedAt,
        e.UpdatedBy
    };
}
