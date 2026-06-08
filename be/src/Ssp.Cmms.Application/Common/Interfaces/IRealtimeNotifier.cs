namespace Ssp.Cmms.Application.Common.Interfaces;

public interface IRealtimeNotifier
{
    Task WorkOrderStatusChangedAsync(object workOrder, CancellationToken ct = default);
    Task AlertCreatedAsync(object alert, CancellationToken ct = default);
    Task DashboardKpiUpdatedAsync(CancellationToken ct = default);
}
