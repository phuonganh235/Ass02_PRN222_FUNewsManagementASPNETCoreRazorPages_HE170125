using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Web.Hubs
{
    public class NotificationHub : Hub
    {
        // Group names based on roles
        private const string GROUP_ADMIN = "Group_Admin";
        private const string GROUP_STAFF = "Group_Staff";
        private const string GROUP_LECTURER = "Group_Lecturer";

        public override async Task OnConnectedAsync()
        {
            var userRole = Context.GetHttpContext()?.Session.GetInt32("UserRole");
            var userName = Context.GetHttpContext()?.Session.GetString("UserName");

            if (userRole.HasValue)
            {
                string groupName = userRole.Value switch
                {
                    0 => GROUP_ADMIN,
                    1 => GROUP_STAFF,
                    2 => GROUP_LECTURER,
                    _ => string.Empty
                };

                if (!string.IsNullOrEmpty(groupName))
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userRole = Context.GetHttpContext()?.Session.GetInt32("UserRole");

            if (userRole.HasValue)
            {
                string groupName = userRole.Value switch
                {
                    0 => GROUP_ADMIN,
                    1 => GROUP_STAFF,
                    2 => GROUP_LECTURER,
                    _ => string.Empty
                };

                if (!string.IsNullOrEmpty(groupName))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        // Send notification to all users
        public async Task SendNotificationToAll(string message, string type)
        {
            await Clients.All.SendAsync("ReceiveNotification", message, type);
        }

        // Send notification to Admin group only
        public async Task SendNotificationToAdmin(string message, string type)
        {
            await Clients.Group(GROUP_ADMIN).SendAsync("ReceiveNotification", message, type);
        }

        // Send notification to Staff group
        public async Task SendNotificationToStaff(string message, string type)
        {
            await Clients.Group(GROUP_STAFF).SendAsync("ReceiveNotification", message, type);
        }

        // Send notification to Lecturer group
        public async Task SendNotificationToLecturer(string message, string type)
        {
            await Clients.Group(GROUP_LECTURER).SendAsync("ReceiveNotification", message, type);
        }

        // Account CRUD notifications
        public async Task NotifyAccountCreated(string accountName)
        {
            await Clients.Group(GROUP_STAFF).SendAsync("ReceiveNotification",
                $"Admin has added a new account: {accountName}", "info");
        }

        public async Task NotifyAccountUpdated(string accountName)
        {
            await Clients.All.SendAsync("ReceiveNotification",
                $"Account '{accountName}' has been updated", "info");
        }

        public async Task NotifyAccountDeleted(string accountName)
        {
            await Clients.All.SendAsync("ReceiveNotification",
                $"Account '{accountName}' has been deleted", "warning");
        }

        // Category CRUD notifications
        public async Task NotifyCategoryCreated(string categoryName)
        {
            await Clients.Groups(GROUP_ADMIN, GROUP_STAFF).SendAsync("ReceiveNotification",
                $"New category created: {categoryName}", "success");
            await Clients.All.SendAsync("RefreshCategories");
        }

        public async Task NotifyCategoryUpdated(string categoryName)
        {
            await Clients.Groups(GROUP_ADMIN, GROUP_STAFF).SendAsync("ReceiveNotification",
                $"Category '{categoryName}' has been updated", "info");
            await Clients.All.SendAsync("RefreshCategories");
        }

        public async Task NotifyCategoryDeleted(string categoryName)
        {
            await Clients.Groups(GROUP_ADMIN, GROUP_STAFF).SendAsync("ReceiveNotification",
                $"Category '{categoryName}' has been deleted", "warning");
            await Clients.All.SendAsync("RefreshCategories");
        }

        public async Task NotifyCategoryOrderChanged()
        {
            await Clients.All.SendAsync("RefreshCategoryOrder");
        }

        // News Article CRUD notifications
        public async Task NotifyNewsArticleCreated(string newsTitle, string authorName)
        {
            await Clients.All.SendAsync("ReceiveNotification",
                $"📰 New Article Published: '{newsTitle}' by {authorName}", "success");
            await Clients.All.SendAsync("RefreshNewsList");
            await Clients.Group(GROUP_ADMIN).SendAsync("RefreshDashboard");
        }

        public async Task NotifyNewsArticleUpdated(string newsTitle)
        {
            await Clients.All.SendAsync("ReceiveNotification",
                $"Article '{newsTitle}' has been updated", "info");
            await Clients.All.SendAsync("RefreshNewsList");
        }

        public async Task NotifyNewsArticleDeleted(string newsTitle)
        {
            await Clients.All.SendAsync("ReceiveNotification",
                $"⚠️ Article '{newsTitle}' has been deleted", "warning");
            await Clients.All.SendAsync("RefreshNewsList");
            await Clients.Group(GROUP_ADMIN).SendAsync("RefreshDashboard");
        }

        // Tag CRUD notifications
        public async Task NotifyTagCreated(string tagName)
        {
            await Clients.Groups(GROUP_ADMIN, GROUP_STAFF).SendAsync("ReceiveNotification",
                $"New tag created: {tagName}", "success");
            await Clients.All.SendAsync("RefreshTags");
        }

        public async Task NotifyTagDeleted(string tagName)
        {
            await Clients.All.SendAsync("ReceiveNotification",
                $"Tag '{tagName}' was deleted by Admin", "warning");
            await Clients.All.SendAsync("RefreshTags");
        }

        // Comment notifications
        public async Task SendCommentToArticle(string newsArticleId, string commentId, string accountName, string content, string timeAgo)
        {
            await Clients.All.SendAsync("ReceiveComment", newsArticleId, commentId, accountName, content, timeAgo);
        }

        public async Task NotifyCommentDeleted(int commentId, string newsArticleId)
        {
            await Clients.All.SendAsync("CommentDeleted", commentId, newsArticleId);
        }

        // Live editing notification
        public async Task NotifyUserEditing(string newsArticleId, string userName)
        {
            await Clients.Group(GROUP_ADMIN).SendAsync("ReceiveEditingNotification",
                $"{userName} is currently editing article #{newsArticleId}");
        }

        // Dashboard update
        public async Task RefreshDashboard()
        {
            await Clients.Group(GROUP_ADMIN).SendAsync("RefreshDashboard");
        }
    }
}