using Ssp.Cmms.Domain.Enums;

namespace Ssp.Cmms.Domain.Services;

/// <summary>
/// Valid transitions for the work order lifecycle:
/// Draft -> Assigned -> InProgress -> OnHold -> Completed -> Closed.
/// OnHold can resume to InProgress; Closed is terminal.
/// </summary>
public static class WorkOrderStatusMachine
{
    private static readonly Dictionary<WorkOrderStatus, WorkOrderStatus[]> Transitions =
        new()
        {
            [WorkOrderStatus.Draft] = [WorkOrderStatus.Assigned],
            [WorkOrderStatus.Assigned] =
                [WorkOrderStatus.InProgress, WorkOrderStatus.Draft],
            [WorkOrderStatus.InProgress] =
                [WorkOrderStatus.OnHold, WorkOrderStatus.Completed],
            [WorkOrderStatus.OnHold] = [WorkOrderStatus.InProgress],
            [WorkOrderStatus.Completed] = [WorkOrderStatus.Closed],
            [WorkOrderStatus.Closed] = []
        };

    public static bool CanTransition(WorkOrderStatus from, WorkOrderStatus to) =>
        Transitions.TryGetValue(from, out var allowed) && allowed.Contains(to);

    public static IReadOnlyCollection<WorkOrderStatus> AllowedNext(
        WorkOrderStatus from) =>
        Transitions.TryGetValue(from, out var allowed)
            ? allowed
            : Array.Empty<WorkOrderStatus>();
}
