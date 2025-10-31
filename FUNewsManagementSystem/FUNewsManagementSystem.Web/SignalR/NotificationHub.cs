using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Web.SignalR
{
    // Hub trung tâm realtime
    // - Gửi thông báo CRUD Account / Category / Article
    // - Broadcast update Dashboard
    // - Gửi cảnh báo khi xoá / cấm / vv
    public class NotificationHub : Hub
    {
        // Gửi message đến tất cả client đã kết nối
        public async Task Broadcast(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        // Gửi message đến group theo role
        public async Task BroadcastToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("ReceiveNotification", message);
        }

        // Cho người dùng join group theo role sau khi login
        public override async Task OnConnectedAsync()
        {
            // Ở phần Auth mình sẽ set group dựa theo role trong Context.Items/Claims/Session
            // Tạm thời: chưa join group cụ thể
            await base.OnConnectedAsync();
        }
    }
}
