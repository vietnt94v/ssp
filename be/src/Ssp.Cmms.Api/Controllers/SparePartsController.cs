using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Exceptions;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/spare-parts")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class SparePartsController : ControllerBase
{
    private readonly IApplicationDbContext _db;

    public SparePartsController(IApplicationDbContext db)
    {
        _db = db;
    }

    public record SparePartInput(
        string Code,
        string Name,
        decimal UnitCost,
        int StockQuantity,
        int ReorderLevel);

    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var query = _db.SpareParts.AsNoTracking();
        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(p => p.Code)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => Project(p))
            .ToListAsync(ct);

        return Ok(new { items, totalCount = total, page, pageSize });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var entity = await _db.SpareParts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException(nameof(SparePart), id);
        return Ok(Project(entity));
    }

    [HttpPost]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Create(
        [FromBody] SparePartInput input,
        CancellationToken ct)
    {
        var entity = new SparePart
        {
            Code = input.Code,
            Name = input.Name,
            UnitCost = input.UnitCost,
            StockQuantity = input.StockQuantity,
            ReorderLevel = input.ReorderLevel
        };
        _db.SpareParts.Add(entity);
        await _db.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id },
            Project(entity));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "RequireManagerOrAdmin")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] SparePartInput input,
        CancellationToken ct)
    {
        var entity = await _db.SpareParts
            .FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new NotFoundException(nameof(SparePart), id);

        entity.Code = input.Code;
        entity.Name = input.Name;
        entity.UnitCost = input.UnitCost;
        entity.StockQuantity = input.StockQuantity;
        entity.ReorderLevel = input.ReorderLevel;
        await _db.SaveChangesAsync(ct);
        return Ok(Project(entity));
    }

    private static object Project(SparePart p) => new
    {
        p.Id,
        p.Code,
        p.Name,
        p.UnitCost,
        p.StockQuantity,
        p.ReorderLevel,
        p.CreatedAt,
        p.CreatedBy,
        p.UpdatedAt,
        p.UpdatedBy
    };
}
