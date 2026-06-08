using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Application.Common.Interfaces;

namespace Ssp.Cmms.Api.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize(Policy = "RequireTechnicianOrAbove")]
public class SettingsController : ControllerBase
{
    private readonly IApplicationDbContext _db;

    public SettingsController(IApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("users")]
    [Authorize(Policy = "RequireAdmin")]
    public async Task<IActionResult> Users(CancellationToken ct)
    {
        var users = await _db.Users
            .AsNoTracking()
            .OrderBy(u => u.FullName)
            .Select(u => new
            {
                u.Id,
                u.Email,
                u.FullName,
                u.Role
            })
            .ToListAsync(ct);
        return Ok(users);
    }

    [HttpGet("categories")]
    public async Task<IActionResult> Categories(CancellationToken ct)
    {
        var categories = await _db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.Name, c.Description })
            .ToListAsync(ct);
        return Ok(categories);
    }

    [HttpGet("locations")]
    public async Task<IActionResult> Locations(CancellationToken ct)
    {
        var locations = await _db.Locations
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .Select(l => new { l.Id, l.Name, l.Area })
            .ToListAsync(ct);
        return Ok(locations);
    }
}
