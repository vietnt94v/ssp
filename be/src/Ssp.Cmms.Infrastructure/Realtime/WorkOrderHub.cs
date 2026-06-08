using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Ssp.Cmms.Infrastructure.Realtime;

[Authorize]
public class WorkOrderHub : Hub
{
    public const string Path = "/hubs/work-orders";

    public async Task JoinGroup(string group)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    public async Task LeaveGroup(string group)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }
}
