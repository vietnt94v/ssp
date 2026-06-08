using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Infrastructure.Persistence.Configurations;

public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> b)
    {
        b.ToTable("work_orders");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Number).HasColumnName("number").HasMaxLength(32).IsRequired();
        b.Property(x => x.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(32);
        b.Property(x => x.Priority).HasColumnName("priority").HasConversion<string>().HasMaxLength(32);
        b.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(32);
        b.Property(x => x.EquipmentId).HasColumnName("equipment_id");
        b.Property(x => x.AssignedTechnicianId).HasColumnName("assigned_technician_id");
        b.Property(x => x.ScheduleId).HasColumnName("schedule_id");
        b.Property(x => x.Description).HasColumnName("description");
        b.Property(x => x.Deadline).HasColumnName("deadline");
        b.Property(x => x.StartedAt).HasColumnName("started_at");
        b.Property(x => x.CompletedAt).HasColumnName("completed_at");
        b.Property(x => x.IsDeleted).HasColumnName("is_deleted");
        b.Property(x => x.DeletedAt).HasColumnName("deleted_at");
        b.Property(x => x.DeletedBy).HasColumnName("deleted_by");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.CreatedBy).HasColumnName("created_by");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.Property(x => x.UpdatedBy).HasColumnName("updated_by");

        b.HasIndex(x => x.Number).IsUnique();
        b.HasIndex(x => x.Status);
        b.HasIndex(x => x.Priority);
        b.HasIndex(x => x.EquipmentId);
        b.HasIndex(x => x.AssignedTechnicianId);
        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasOne(x => x.Equipment)
            .WithMany(e => e.WorkOrders)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Restrict);

        b.HasOne(x => x.AssignedTechnician)
            .WithMany(t => t.WorkOrders)
            .HasForeignKey(x => x.AssignedTechnicianId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasOne(x => x.Schedule)
            .WithMany(s => s.GeneratedWorkOrders)
            .HasForeignKey(x => x.ScheduleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

public class WorkOrderChecklistItemConfiguration
    : IEntityTypeConfiguration<WorkOrderChecklistItem>
{
    public void Configure(EntityTypeBuilder<WorkOrderChecklistItem> b)
    {
        b.ToTable("work_order_checklist_items");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.WorkOrderId).HasColumnName("work_order_id");
        b.Property(x => x.Description).HasColumnName("description").IsRequired();
        b.Property(x => x.IsDone).HasColumnName("is_done");
        b.Property(x => x.SortOrder).HasColumnName("sort_order");

        b.HasOne(x => x.WorkOrder)
            .WithMany(w => w.Checklist)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class WorkOrderPartConfiguration : IEntityTypeConfiguration<WorkOrderPart>
{
    public void Configure(EntityTypeBuilder<WorkOrderPart> b)
    {
        b.ToTable("work_order_parts");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.WorkOrderId).HasColumnName("work_order_id");
        b.Property(x => x.SparePartId).HasColumnName("spare_part_id");
        b.Property(x => x.Quantity).HasColumnName("quantity");
        b.Property(x => x.UnitCost).HasColumnName("unit_cost").HasPrecision(18, 2);

        b.HasOne(x => x.WorkOrder)
            .WithMany(w => w.Parts)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.SparePart)
            .WithMany(s => s.WorkOrderParts)
            .HasForeignKey(x => x.SparePartId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class CostEntryConfiguration : IEntityTypeConfiguration<CostEntry>
{
    public void Configure(EntityTypeBuilder<CostEntry> b)
    {
        b.ToTable("cost_entries");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.WorkOrderId).HasColumnName("work_order_id");
        b.Property(x => x.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(32);
        b.Property(x => x.Amount).HasColumnName("amount").HasPrecision(18, 2);
        b.Property(x => x.Description).HasColumnName("description");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.CreatedBy).HasColumnName("created_by");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.Property(x => x.UpdatedBy).HasColumnName("updated_by");

        b.HasOne(x => x.WorkOrder)
            .WithMany(w => w.Costs)
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
