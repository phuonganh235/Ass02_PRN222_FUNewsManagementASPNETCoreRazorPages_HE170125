using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Web.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
