using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ssp.Cmms.Application.Common.Interfaces;
using Ssp.Cmms.Domain.Entities;
using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Infrastructure.Jobs;

/// <summary>
/// Daily Hangfire job: generates preventive work orders from schedules that are
/// due, advances the schedule's next due date, and raises overdue/PM alerts.
/// </summary>
public class GeneratePreventiveWorkOrdersJob
{
    private readonly IApplicationDbContext _db;
    private readonly IRealtimeNotifier _notifier;
    private readonly ILogger<GeneratePreventiveWorkOrdersJob> _logger;

    public GeneratePreventiveWorkOrdersJob(
        IApplicationDbContext db,
        IRealtimeNotifier notifier,
        ILogger<GeneratePreventiveWorkOrdersJob> logger)
    {
        _db = db;
        _notifier = notifier;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var dueSchedules = await _db.MaintenanceSchedules
            .Where(s => s.IsActive && s.NextDueDate <= today)
            .ToListAsync(cancellationToken);

        if (dueSchedules.Count == 0)
        {
            _logger.LogInformation("PM job: no schedules due.");
            return;
        }

        var year = DateTime.UtcNow.Year;
        var sequence = await _db.WorkOrders
            .IgnoreQueryFilters()
            .CountAsync(w => w.Number.StartsWith($"WO-{year}-"), cancellationToken);

        foreach (var schedule in dueSchedules)
        {
            sequence++;
            var workOrder = new WorkOrder
            {
                Number = $"WO-{year}-{sequence:D4}",
                Type = WorkOrderType.Preventive,
                Priority = WorkOrderPriority.Medium,
                Status = WorkOrderStatus.Draft,
                EquipmentId = schedule.EquipmentId,
                ScheduleId = schedule.Id,
                Description = $"Auto-generated PM: {schedule.Title}",
                Deadline = schedule.NextDueDate
                    .ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc)
            };
            _db.WorkOrders.Add(workOrder);

            schedule.NextDueDate = Advance(schedule, schedule.NextDueDate);

            _db.Alerts.Add(new Alert
            {
                Type = AlertType.PmDue,
                Message = $"Preventive work order generated for '{schedule.Title}'.",
                EntityId = schedule.Id,
                EntityType = nameof(MaintenanceSchedule),
                CreatedAt = DateTimeOffset.UtcNow
            });
        }

        await _db.SaveChangesAsync(cancellationToken);
        await _notifier.DashboardKpiUpdatedAsync(cancellationToken);

        _logger.LogInformation(
            "PM job: generated {Count} preventive work orders.",
            dueSchedules.Count);
    }

    private static DateOnly Advance(MaintenanceSchedule schedule, DateOnly from) =>
        schedule.Frequency switch
        {
            ScheduleFrequency.Daily => from.AddDays(schedule.IntervalValue),
            ScheduleFrequency.Weekly => from.AddDays(7 * schedule.IntervalValue),
            ScheduleFrequency.Monthly => from.AddMonths(schedule.IntervalValue),
            ScheduleFrequency.ByMeter => from.AddDays(30 * schedule.IntervalValue),
            _ => from.AddMonths(1)
        };
}
