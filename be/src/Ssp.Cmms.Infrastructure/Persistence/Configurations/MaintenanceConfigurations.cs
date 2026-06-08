using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ssp.Cmms.Domain.Entities;

namespace Ssp.Cmms.Infrastructure.Persistence.Configurations;

public class TechnicianConfiguration : IEntityTypeConfiguration<Technician>
{
    public void Configure(EntityTypeBuilder<Technician> b)
    {
        b.ToTable("technicians");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.UserId).HasColumnName("user_id");
        b.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        b.Property(x => x.Department).HasColumnName("department").HasMaxLength(150);
        b.Property(x => x.Rating).HasColumnName("rating").HasPrecision(3, 2);
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.CreatedBy).HasColumnName("created_by");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.Property(x => x.UpdatedBy).HasColumnName("updated_by");

        b.HasOne(x => x.User)
            .WithOne(u => u.Technician)
            .HasForeignKey<Technician>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TechnicianSkillConfiguration
    : IEntityTypeConfiguration<TechnicianSkill>
{
    public void Configure(EntityTypeBuilder<TechnicianSkill> b)
    {
        b.ToTable("technician_skills");
        b.HasKey(x => new { x.TechnicianId, x.SkillId });
        b.Property(x => x.TechnicianId).HasColumnName("technician_id");
        b.Property(x => x.SkillId).HasColumnName("skill_id");

        b.HasOne(x => x.Technician)
            .WithMany(t => t.TechnicianSkills)
            .HasForeignKey(x => x.TechnicianId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.Skill)
            .WithMany(s => s.TechnicianSkills)
            .HasForeignKey(x => x.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MaintenanceScheduleConfiguration
    : IEntityTypeConfiguration<MaintenanceSchedule>
{
    public void Configure(EntityTypeBuilder<MaintenanceSchedule> b)
    {
        b.ToTable("maintenance_schedules");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.EquipmentId).HasColumnName("equipment_id");
        b.Property(x => x.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
        b.Property(x => x.Frequency).HasColumnName("frequency").HasConversion<string>().HasMaxLength(32);
        b.Property(x => x.IntervalValue).HasColumnName("interval_value");
        b.Property(x => x.MeterThreshold).HasColumnName("meter_threshold").HasPrecision(18, 2);
        b.Property(x => x.NextDueDate).HasColumnName("next_due_date");
        b.Property(x => x.IsActive).HasColumnName("is_active");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.CreatedBy).HasColumnName("created_by");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.Property(x => x.UpdatedBy).HasColumnName("updated_by");

        b.HasIndex(x => x.NextDueDate);

        b.HasOne(x => x.Equipment)
            .WithMany(e => e.Schedules)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class MaintenanceLogConfiguration : IEntityTypeConfiguration<MaintenanceLog>
{
    public void Configure(EntityTypeBuilder<MaintenanceLog> b)
    {
        b.ToTable("maintenance_logs");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.EquipmentId).HasColumnName("equipment_id");
        b.Property(x => x.WorkOrderId).HasColumnName("work_order_id");
        b.Property(x => x.CompletedAt).HasColumnName("completed_at");
        b.Property(x => x.Summary).HasColumnName("summary");
        b.Property(x => x.DowntimeMinutes).HasColumnName("downtime_minutes");

        b.HasIndex(x => x.EquipmentId);

        b.HasOne(x => x.Equipment)
            .WithMany(e => e.Logs)
            .HasForeignKey(x => x.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasOne(x => x.WorkOrder)
            .WithMany()
            .HasForeignKey(x => x.WorkOrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class SparePartConfiguration : IEntityTypeConfiguration<SparePart>
{
    public void Configure(EntityTypeBuilder<SparePart> b)
    {
        b.ToTable("spare_parts");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Code).HasColumnName("code").HasMaxLength(64).IsRequired();
        b.Property(x => x.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
        b.Property(x => x.UnitCost).HasColumnName("unit_cost").HasPrecision(18, 2);
        b.Property(x => x.StockQuantity).HasColumnName("stock_quantity");
        b.Property(x => x.ReorderLevel).HasColumnName("reorder_level");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.CreatedBy).HasColumnName("created_by");
        b.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        b.Property(x => x.UpdatedBy).HasColumnName("updated_by");

        b.HasIndex(x => x.Code).IsUnique();
    }
}

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> b)
    {
        b.ToTable("alerts");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("id");
        b.Property(x => x.Type).HasColumnName("type").HasConversion<string>().HasMaxLength(32);
        b.Property(x => x.Message).HasColumnName("message").IsRequired();
        b.Property(x => x.EntityId).HasColumnName("entity_id");
        b.Property(x => x.EntityType).HasColumnName("entity_type").HasMaxLength(64);
        b.Property(x => x.IsAcknowledged).HasColumnName("is_acknowledged");
        b.Property(x => x.CreatedAt).HasColumnName("created_at");
        b.Property(x => x.AcknowledgedAt).HasColumnName("acknowledged_at");
        b.Property(x => x.AcknowledgedBy).HasColumnName("acknowledged_by");

        b.HasIndex(x => x.IsAcknowledged);
    }
}
