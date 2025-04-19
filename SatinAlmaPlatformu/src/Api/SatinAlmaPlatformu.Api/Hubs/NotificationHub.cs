using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SatinAlmaPlatformu.Api.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string userId, string message, string type)
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }

        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendNotificationToGroup(string groupName, string message, string type)
        {
            await Clients.Group(groupName).SendAsync("ReceiveNotification", message, type);
        }
    }
} 