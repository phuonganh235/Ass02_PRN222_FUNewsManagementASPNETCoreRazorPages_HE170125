using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace FUNewsManagementSystem.Web.Hubs
{
    public class CommentHub : Hub
    {
        // client sẽ join vào group theo ArticleID
        public async Task JoinArticleRoom(string articleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"article_{articleId}");
        }

        public async Task LeaveArticleRoom(string articleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"article_{articleId}");
        }

        public async Task SendComment(string articleId, string userName, string message)
        {
            // broadcast cho tất cả user đang xem cùng article
            await Clients.Group($"article_{articleId}")
                .SendAsync("ReceiveComment", userName, message, DateTime.Now.ToString("HH:mm:ss"));
        }
    }
}
