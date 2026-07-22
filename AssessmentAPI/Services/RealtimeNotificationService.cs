using Assessment.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Assessment.API.Services
{
    public class RealtimeNotificationService
    {
        private readonly IHubContext<AssessmentNotificationHub> _hub;
        public RealtimeNotificationService(IHubContext<AssessmentNotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task BroadcastAsync(string message)
        {
            await _hub.Clients.All.SendAsync("ReceiveNotification", message);

        }

        public async Task SendToUserAsync(string userId,string message)
        {
            await _hub.Clients.Group(userId).SendAsync("ReceiveNotification", message);

        }
    }
}
