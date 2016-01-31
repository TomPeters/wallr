using Microsoft.AspNet.SignalR;

namespace Wallr.UI.SignalR
{
    public class WallrHub : Hub
    {
        public void SendEvent(string eventName, object eventArgs)
        {
            Clients.All.addMessage(eventName, eventArgs);
        }
    }
}