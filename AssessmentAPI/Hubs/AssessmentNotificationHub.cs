using Microsoft.AspNetCore.SignalR;

namespace Assessment.API.Hubs
{
    public class AssessmentNotificationHub: Hub
    {

        public override async Task OnConnectedAsync()
        {
            var user = Context.GetHttpContext()?.Request.Query["UserId"];
            if(!string.IsNullOrEmpty(user))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId,user!);
            }
            await base.OnConnectedAsync();
        }
    }
}
