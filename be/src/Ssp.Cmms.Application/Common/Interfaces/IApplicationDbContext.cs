using Microsoft.EntityFrameworkCore;
using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Category> Categories { get; }
    DbSet<Location> Locations { get; }
    DbSet<Skill> Skills { get; }
    DbSet<Equipment> Equipment { get; }
    DbSet<Technician> Technicians { get; }
    DbSet<TechnicianSkill> TechnicianSkills { get; }
    DbSet<WorkOrder> WorkOrders { get; }
    DbSet<WorkOrderChecklistItem> WorkOrderChecklistItems { get; }
    DbSet<WorkOrderPart> WorkOrderParts { get; }
    DbSet<MaintenanceSchedule> MaintenanceSchedules { get; }
    DbSet<CostEntry> CostEntries { get; }
    DbSet<MaintenanceLog> MaintenanceLogs { get; }
    DbSet<SparePart> SpareParts { get; }
    DbSet<Alert> Alerts { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
