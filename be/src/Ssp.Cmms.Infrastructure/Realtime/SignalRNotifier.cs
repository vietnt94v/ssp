using Microsoft.AspNetCore.SignalR;
using Ssp.Cmms.Application.Common.Interfaces;

namespace Ssp.Cmms.Infrastructure.Realtime;

public class SignalRNotifier : IRealtimeNotifier
{
    private readonly IHubContext<WorkOrderHub> _hub;

    public SignalRNotifier(IHubContext<WorkOrderHub> hub)
    {
        _hub = hub;
    }

    public Task WorkOrderStatusChangedAsync(
        object workOrder,
        CancellationToken ct = default) =>
        _hub.Clients.All.SendAsync("WorkOrderStatusChanged", workOrder, ct);

    public Task AlertCreatedAsync(object alert, CancellationToken ct = default) =>
        _hub.Clients.All.SendAsync("AlertCreated", alert, ct);

    public Task DashboardKpiUpdatedAsync(CancellationToken ct = default) =>
        _hub.Clients.All.SendAsync("DashboardKpiUpdated", ct);
}
